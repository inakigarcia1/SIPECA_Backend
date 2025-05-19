using SIPECA.Aplicacion.DTOs;
using SIPECA.Dominio.Distribuciones.Continuas;
using SIPECA.Dominio.Distribuciones.Discretas;
using SIPECA.Dominio.Generadores;

namespace SIPECA.Aplicacion;

public class SimulacionService
{
    private static readonly IGenerador Generador = new CongruencialMixto();
    private readonly Normal _distribucionNormal = new(Generador);
    private readonly Exponencial _distribucionExponencial = new(Generador);
    private readonly Poisson _distribucionPoisson = new(Generador);
    private readonly Uniforme _distribucionUniforme = new(Generador);
    public static ResultadosSimulacion Simular(ParametrosSimulacion parametros)
    {
        // Lógica de simulación
        return new ResultadosSimulacion();
    }
}
