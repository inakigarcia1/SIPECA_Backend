namespace SIPECA.Aplicacion.DTOs.Response;

public class ResultadoSimulacion
{
    public ulong DiasTotales { get; set; }
    public int GeneracionesTotales { get; set; }
    public double HectareasInfectadasFinales { get; set; }
    public double PerasSanasFinales { get; set; }
    public double PerasInfectadasFinales { get; set; }
    public double CostoTotalTratamientoQuimico { get; set; }
    public double CostoTotalTratamientoFeromonas { get; set; }
    public double DineroFinalGanado { get; set; }
    public double DineroFinalPerdido { get; set; }

    public List<ResultadosGeneracion> ResultadosPorGeneracion { get; set; }

    public ResultadoSimulacion(List<ResultadosGeneracion> resultadosPorGeneracion)
    {
        ResultadosPorGeneracion = resultadosPorGeneracion;
    }
}
