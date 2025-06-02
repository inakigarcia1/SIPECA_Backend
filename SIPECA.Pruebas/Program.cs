using SIPECA.Aplicacion;
using SIPECA.Aplicacion.DTOs;
using SIPECA.Aplicacion.DTOs.Request;

namespace SIPECA.Pruebas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var resultado = SimulacionService.Simular(new ParametrosSimulacion()
            {
                CantidadHectareas = 900,
                PlantasPorHectarea = 500,
                HectareasInfectadas = 3,
                CostoTratamientoFeromonasPorHectarea = 1000,
                CostoTratamientoQuimicoPorHectarea = 2000,
                PrecioPera = 1000,
                AplicarQuimicos = false,
                AplicarFeromonas = false
            });
            Console.WriteLine(resultado.ToString());
        }
    }
}
