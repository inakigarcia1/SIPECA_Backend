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
        double peralesPorHectarea = parametros.PlantasPorHectarea;
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
        var plagas = ObtenerPlagas(hectareasInfectadas, peralesPorHectarea);
        var peralesTotales = cantidadHectareas * peralesPorHectarea;

        if (plagas.cantidadHembras == 0) plagas.cantidadHembras = 1; // Aseguramos que haya al menos una hembra para la simulación

        var perasEnPlantas = new List<Tuple<double, double>>(); // Acá se almacenan todas las plantas y los pesos de sus peras

        // Recorrer lo sano y guardar las plantas
        var hectareasSanas = cantidadHectareas - hectareasInfectadas;
        for (int i = 0; i < hectareasSanas; i++)
        {
            for (int j = 0; j < peralesPorHectarea; j++)
            {
                var perasPorPlanta = DistribucionUniforme.GenerarVariableAleatoria(70, 100);
                var pesoPera = DistribucionUniforme.GenerarVariableAleatoria(0.120, 0.250, truncar: false);
                var pesoPerasPlanta = pesoPera * perasPorPlanta;
                perasEnPlantas.Add(new Tuple<double, double>(perasPorPlanta, pesoPerasPlanta));

                perasTotales += perasPorPlanta;
                pesoTotalDePeras += pesoPerasPlanta;
            }
        }

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
            if (plagas.cantidadHembras == 0) plagas.cantidadHembras = 1; // Aseguramos que haya al menos una hembra para la simulación

            var diaGeneracion = 0d;

            diaGeneracion += DistribucionUniforme.GenerarVariableAleatoria(6, 11);
            diaGeneracion += DistribucionUniforme.GenerarVariableAleatoria(15, 25);
            diaGeneracion += DistribucionUniforme.GenerarVariableAleatoria(7, 9);

            dia += diaGeneracion;

            /*
             Recorrer las plantas y dañarlas.
                Item1 = cantidad de peras en la planta
                Item2 = peso de todas las peras de esa planta
             */

            var perasEnPlantasCopia = new List<Tuple<double, double>>(perasEnPlantas);
            foreach (var planta in perasEnPlantasCopia.TakeWhile(peras => cantidadHuevos != 0))
            {
                double perasDañadasDelPeral;
                if (cantidadHuevos - planta.Item1 < 0) // Dañar parcialmente la planta
                {
                    perasInfectadas += cantidadHuevos;
                    perasDañadasDelPeral = cantidadHuevos;
                    cantidadHuevos = 0;
                }
                else // Dañar completamente la planta
                {
                    perasInfectadas += planta.Item1;
                    cantidadHuevos -= planta.Item1;
                    perasDañadasDelPeral = planta.Item1;
                }
                pesoPerasInfectadas += planta.Item2;
                peralesInfectados += perasDañadasDelPeral / planta.Item1;
                perasEnPlantas.Remove(planta);
                if (peralesInfectados >= peralesTotales) break;
            }

            hectareasInfectadas += peralesInfectados / peralesPorHectarea;

            var pesoPerasSanas = pesoTotalDePeras - pesoPerasInfectadas;
            perasSanas = perasTotales - perasInfectadas;

            var ultimo = ResultadosPorGeneracion.LastOrDefault();

            double costoTratamientoQuimico = ultimo is null ? costoQuimicoPorHectarea * cantidadHectareas : ultimo.CostoTratamientoQuimico + costoQuimicoPorHectarea * cantidadHectareas;
            double costoFeromonas = ultimo is null ? costoFeromonasPorHectarea * cantidadHectareas : ultimo.CostoTratamientoFeromonas + costoFeromonasPorHectarea * cantidadHectareas;

            if (pesoPerasSanas < 0 || perasSanas < 0 || hectareasInfectadas >= cantidadHectareas || peralesInfectados >= peralesTotales)  // Todas las plantas infectadas
            {
                perasSanas = 0;
                pesoPerasSanas = 0;
                pesoPerasInfectadas = pesoTotalDePeras;
                perasInfectadas = perasTotales;
                hectareasInfectadas = cantidadHectareas;
                hectareasSanas = 0;
                ResultadosPorGeneracion.Add(new ResultadosGeneracion()
                {
                    Generacion = generacion,
                    Dias = (int)diaGeneracion,
                    HectareasInfectadas = hectareasInfectadas,
                    HectareasSanas = hectareasSanas,
                    PerasSanas = perasSanas,
                    PerasInfectadas = perasInfectadas,
                    Ganancia = (pesoPerasSanas * precioPeraPorTonelada) / 1000,
                    Perdida = (pesoPerasInfectadas * precioPeraPorTonelada) / 1000,
                    CostoTratamientoQuimico = aplicarQuimicos ? costoTratamientoQuimico : 0,
                    CostoTratamientoFeromonas = aplicarFeromonas ? costoFeromonas : 0
                });
                break;
            }

            hectareasSanas = cantidadHectareas - hectareasInfectadas;

            ResultadosPorGeneracion.Add(new ResultadosGeneracion()
            {
                Generacion = generacion,
                Dias = (int)diaGeneracion,
                HectareasInfectadas = hectareasInfectadas,
                HectareasSanas = hectareasSanas,
                PerasSanas = perasSanas,
                PerasInfectadas = perasInfectadas,
                Ganancia = (pesoPerasSanas * precioPeraPorTonelada) / 1000,
                Perdida = (pesoPerasInfectadas * precioPeraPorTonelada) / 1000,
                CostoTratamientoQuimico = aplicarQuimicos ? costoTratamientoQuimico : 0,
                CostoTratamientoFeromonas = aplicarFeromonas ? costoFeromonas : 0
            });
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

