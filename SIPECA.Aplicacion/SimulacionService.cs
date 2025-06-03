using SIPECA.Aplicacion.DTOs.Request;
using SIPECA.Aplicacion.DTOs.Response;
using SIPECA.Dominio.Distribuciones.Continuas;
using SIPECA.Dominio.Distribuciones.Discretas;
using SIPECA.Dominio.Generadores;

namespace SIPECA.Aplicacion;

public class SimulacionService
{
    private static readonly IGenerador Generador = new CongruencialMixto();
    private static readonly Normal DistribucionNormal = new(Generador);
    private static readonly Exponencial DistribucionExponencial = new(Generador);
    private static readonly Poisson DistribucionPoisson = new(Generador);
    private static readonly Uniforme DistribucionUniforme = new(Generador);
    public static List<ResultadosGeneracion> ResultadosPorGeneracion { get; private set; } = [];

    // La distribución binomial se instancia por cada caso
    public static ResultadoSimulacion Simular(ParametrosSimulacion parametros)
    {
        // Datos iniciales
        ResultadosPorGeneracion.Clear();
        double cantidadHectareas = parametros.CantidadHectareas;
        var precioPeraPorTonelada = parametros.PrecioPera;
        var hectareasInfectadas = parametros.HectareasInfectadas;
        double plantasPorHectarea = parametros.PlantasPorHectarea;
        bool aplicarQuimicos = parametros.AplicarQuimicos;
        bool aplicarFeromonas = parametros.AplicarFeromonas;
        var costoQuimicoPorHectarea = parametros.CostoTratamientoQuimicoPorHectarea;
        var costoFeromonasPorHectarea = parametros.CostoTratamientoFeromonasPorHectarea;

        // Variables a utilizar
        var perasSanas = 0d;
        var perasInfectadas = 0d;
        var perasTotales = 0d;
        var gananciaTotal = 0d;
        var peralesInfectados = 0d;

        var pesoTotalDePeras = 0d;
        var pesoPerasInfectadas = 0d;

        var dia = 0d;

        var plagas = ObtenerPlagas(hectareasInfectadas, plantasPorHectarea);

        var perasEnPlantas = new List<Tuple<double, double>>();

        for (int generacion = 1; generacion <= 3; generacion++)
        {
            double cantidadHuevos = 0;
            for (int i = 0; i < plagas.cantidadHembras; i++)
            {
                cantidadHuevos += generacion == 1 ? DistribucionNormal.GenerarVariableAleatoria(45, 10) : DistribucionNormal.GenerarVariableAleatoria(120, 20);
            }

            cantidadHuevos = FiltrarDiapausa(cantidadHuevos, generacion);

            if (aplicarFeromonas) cantidadHuevos = FiltrarPorFeromonas(cantidadHuevos);
            if (aplicarQuimicos) cantidadHuevos = FiltrarPorInsecticida(cantidadHuevos, generacion);

            plagas.cantidadHembras = DividirSexos(cantidadHuevos).cantidadHembras;

            var diaGeneracion = 0d;

            diaGeneracion += DistribucionUniforme.GenerarVariableAleatoria(6, 11);
            diaGeneracion += DistribucionUniforme.GenerarVariableAleatoria(15, 25);
            diaGeneracion += DistribucionUniforme.GenerarVariableAleatoria(7, 9);

            dia += diaGeneracion;


            for (int i = 0; i < cantidadHectareas - hectareasInfectadas; i++)
            {
                if (generacion != 1)
                {
                    int indicePlanta = 0;
                    foreach (var peras in perasEnPlantas.Where(p => cantidadHuevos - p.Item1 >= 0))
                    {
                        // Dañar la planta
                        perasInfectadas += peras.Item1;
                        pesoPerasInfectadas += peras.Item2;
                        peralesInfectados += cantidadHuevos / peras.Item1;
                        if (cantidadHuevos - peras.Item1 < 0) continue;
                        cantidadHuevos -= peras.Item1;
                        indicePlanta++;

                        if (peralesInfectados >= plantasPorHectarea * cantidadHectareas) break;
                    }
                    perasEnPlantas.RemoveRange(0, indicePlanta);
                    continue;
                }

                for (int j = 0; j < plantasPorHectarea; j++)
                {
                    var perasPorPlanta = DistribucionUniforme.GenerarVariableAleatoria(70, 100);
                    var pesoPera = DistribucionUniforme.GenerarVariableAleatoria(0.120, 0.250, truncar: false);

                    perasTotales += perasPorPlanta;
                    var pesoPerasPlanta = pesoPera * perasPorPlanta;
                    perasEnPlantas.Add(new Tuple<double, double>(perasPorPlanta, pesoPerasPlanta));
                    pesoTotalDePeras += pesoPerasPlanta;

                    // Dañar la planta
                    perasInfectadas += perasPorPlanta;
                    pesoPerasInfectadas += pesoPerasPlanta;
                    peralesInfectados += cantidadHuevos / perasPorPlanta;

                    if (cantidadHuevos - perasPorPlanta < 0) continue;
                    cantidadHuevos -= perasPorPlanta;

                    if (peralesInfectados >= plantasPorHectarea * cantidadHectareas) break;
                }
                gananciaTotal = pesoTotalDePeras * precioPeraPorTonelada / 1000;
            }

            hectareasInfectadas += peralesInfectados / plantasPorHectarea;

            var pesoPerasSanas = pesoTotalDePeras - pesoPerasInfectadas;
            perasSanas = perasTotales - perasInfectadas;

            if (pesoPerasSanas < 0 || perasSanas < 0)
            {
                pesoPerasSanas = 0;
                pesoPerasInfectadas = pesoTotalDePeras;
                perasSanas = 0;
                perasInfectadas = perasTotales;
            }

            var ultimo = ResultadosPorGeneracion.LastOrDefault();

            ResultadosPorGeneracion.Add(new ResultadosGeneracion()
            {
                Generacion = generacion,
                Dias = (int)diaGeneracion,
                HectareasInfectadas = hectareasInfectadas,
                HectareasSanas = cantidadHectareas - hectareasInfectadas,
                PerasSanas = perasSanas,
                PerasInfectadas = perasInfectadas,
                Ganancia = (pesoPerasSanas * precioPeraPorTonelada) / 1000,
                Perdida = (pesoPerasInfectadas * precioPeraPorTonelada) / 1000,
                CostoTratamientoQuimico = costoQuimicoPorHectarea * hectareasInfectadas,
                CostoTratamientoFeromonas = costoFeromonasPorHectarea * hectareasInfectadas
            });

            if (hectareasInfectadas < cantidadHectareas) continue;

            hectareasInfectadas = cantidadHectareas;
            perasInfectadas = perasTotales;
            if (ultimo is not null)
            {
                ultimo.HectareasInfectadas = cantidadHectareas;
                ultimo.PerasInfectadas = perasTotales;
            }
            break;
        }

        var resultadoSimulacion = new ResultadoSimulacion(ResultadosPorGeneracion)
        {
            DiasTotales = (ulong)dia,
            GeneracionesTotales = ResultadosPorGeneracion.Count,
            HectareasInfectadasFinales = hectareasInfectadas,
            PerasSanasFinales = perasSanas,
            PerasInfectadasFinales = ResultadosPorGeneracion.Last().PerasInfectadas,
            CostoTotalTratamientoQuimico = ResultadosPorGeneracion.Last().CostoTratamientoQuimico,
            CostoTotalTratamientoFeromonas = ResultadosPorGeneracion.Last().CostoTratamientoFeromonas,
            DineroFinalGanado = ResultadosPorGeneracion.Last().Ganancia,
            DineroFinalPerdido = ResultadosPorGeneracion.Last().Perdida,
        };

        return resultadoSimulacion;
    }

    private static double FiltrarPorInsecticida(double cantidadHuevos, int generacion)
    {
        double probabilidadMorir = generacion == 1 ? 0.98 : 0.63;

        var cantidadOriginalHuevos = cantidadHuevos;
        var probabilidades = new List<Tuple<double, Action>>()
        {
            new(probabilidadMorir, () =>  cantidadHuevos--),
            new(1 - probabilidadMorir, () => { })
        };

        var binomialInsecticida = new Binomial(Generador, probabilidades);
        binomialInsecticida.RealizarProcedimientoBinomial(cantidadOriginalHuevos);
        return cantidadHuevos;
    }

    private static double FiltrarPorFeromonas(double cantidadHuevos)
    {
        var cantidadOriginalHuevos = cantidadHuevos;
        var probabilidades = new List<Tuple<double, Action>>()
        {
            new(0.9, () =>  cantidadHuevos--),
            new(0.1, () => { })
        };

        var binomialFeromonas = new Binomial(Generador, probabilidades);
        binomialFeromonas.RealizarProcedimientoBinomial(cantidadOriginalHuevos);
        return cantidadHuevos;
    }

    private static (int cantidadMachos, int cantidadHembras) ObtenerPlagas(double hectareasInfectadas, double plantasPorHectarea)
    {
        var cantidadMachos = 0;
        var cantidadHembras = 0;

        var probabilidades = new List<Tuple<double, Action>>()
        {
            new(0.5, () =>  cantidadMachos++),
            new(0.5, () =>  cantidadHembras++)
        };

        var binomialSexoPlaga = new Binomial(Generador, probabilidades);

        var numeroPeras = plantasPorHectarea * hectareasInfectadas;

        binomialSexoPlaga.RealizarProcedimientoBinomial(numeroPeras);
        return (cantidadMachos, cantidadHembras);
    }

    private static (int cantidadMachos, int cantidadHembras) DividirSexos(double cantidadHuevos)
    {
        var cantidadMachos = 0;
        var cantidadHembras = 0;

        var probabilidadesSexo = new List<Tuple<double, Action>>()
        {
            new(0.5, () =>  cantidadMachos++),
            new(0.5, () =>  cantidadHembras++)
        };

        var binomialSexoPlaga = new Binomial(Generador, probabilidadesSexo);

        binomialSexoPlaga.RealizarProcedimientoBinomial((int)cantidadHuevos);
        return (cantidadMachos, cantidadHembras);
    }

    private static double FiltrarDiapausa(double cantidadHuevos, int generacion)
    {
        var cantidadOriginalHuevos = cantidadHuevos;

        double probabilidadCrecer = generacion == 1 ? 0.95 : 0.80;
        double probabilidadDiapausa = generacion == 1 ? 0.5 : 0.2;

        var probabilidadesSexo = new List<Tuple<double, Action?>>()
        {
            new(probabilidadCrecer, null),
            new(probabilidadDiapausa, () => cantidadHuevos -= 1)
        };
        var binomialDiapausa = new Binomial(Generador, probabilidadesSexo);
        binomialDiapausa.RealizarProcedimientoBinomial((int)cantidadOriginalHuevos);
        return cantidadHuevos;
    }
}

