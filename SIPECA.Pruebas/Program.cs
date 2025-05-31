using SIPECA.Aplicacion;

namespace SIPECA.Pruebas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var resultado = SimulacionService.Simular(new Aplicacion.DTOs.ParametrosSimulacion());
            Console.WriteLine(resultado.ToString());
        }
    }
}
