using SIPECA.Aplicacion.DTOs;
using SIPECA.Dominio.Distribuciones.Continuas;
using SIPECA.Dominio.Distribuciones.Discretas;
using SIPECA.Dominio.Generadores;

namespace SIPECA.Aplicacion;

public class SimulacionService
{
    private static readonly IGenerador Generador = new CongruencialMixto();
    private static readonly Normal _distribucionNormal = new(Generador);
    private static readonly Exponencial _distribucionExponencial = new(Generador);
    private static readonly Poisson _distribucionPoisson = new(Generador);
    private static readonly Uniforme _distribucionUniforme = new(Generador);
    // La distribución binomial se instancia por cada caso
    public static ResultadoSimulacion Simular(ParametrosSimulacion parametros)
    {
        // Lógica de simulación

        // Datos iniciales
        var cantidadHectareas = 900d;
        var precioPorTonelada = 800d;
        var toneladasPorHectarea = 27d;
        var hectareasInfectadas = 1.5d;
        var plantasPorHectarea = 500d;

        var perasSanas = 0d;
        var perasInfectadas = 0d;
        var perasTotales = 0d;

        var perasPorHectareaMedia = 0d;

        var peralesSanos = plantasPorHectarea * (cantidadHectareas - hectareasInfectadas);
        var peralesInfectados = 0d;

        var dia = 0d;

        var plagas = ObtenerPlagas(hectareasInfectadas, plantasPorHectarea);

        for (int generacion = 1; generacion <= 3; generacion++)
        {
            double cantidadHuevos = 0;
            for (int i = 0; i < plagas.cantidadHembras; i++)
            {
                cantidadHuevos += generacion == 1 ? _distribucionNormal.GenerarVariableAleatoria(45, 10) : _distribucionNormal.GenerarVariableAleatoria(120, 20);
            }

            cantidadHuevos = FiltrarDiapausa(cantidadHuevos, generacion);
            plagas.cantidadHembras = DividirSexos(cantidadHuevos).cantidadHembras;

            dia += _distribucionUniforme.GenerarVariableAleatoria(6, 11);
            dia += _distribucionUniforme.GenerarVariableAleatoria(15, 25);
            dia += _distribucionUniforme.GenerarVariableAleatoria(7, 9);

            for (int i = 0; i < cantidadHectareas - hectareasInfectadas; i++)
            {
                for (int j = 0; j < plantasPorHectarea; j++)
                {
                    var perasPorPlanta = _distribucionUniforme.GenerarVariableAleatoria(70, 100);
                    perasTotales += perasPorPlanta;

                    if (cantidadHuevos - perasPorPlanta >= 0)
                    {
                        peralesSanos -= 1;
                        perasInfectadas += perasPorPlanta;
                        peralesInfectados++;
                        cantidadHuevos -= perasPorPlanta;
                    }
                }
            }
            perasSanas = perasTotales - perasInfectadas;
            hectareasInfectadas += peralesInfectados / plantasPorHectarea;
        }


        return new ResultadoSimulacion
        {
            CantidadHectareasInfectadas = hectareasInfectadas,
            CantidadHectareasSanas = cantidadHectareas - hectareasInfectadas,
            PerasSanas = perasSanas,
            PerasInfectadas = perasInfectadas,
            DiasTranscurridos = dia
        };
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

