namespace SIPECA.Aplicacion.DTOs.Request;

public class ParametrosSimulacion
{
    public double CantidadHectareas { get; set; }
    public double PlantasPorHectarea { get; set; }
    public double HectareasInfectadas { get; set; }
    public double CostoTratamientoFeromonasPorHectarea { get; set; }
    public double CostoTratamientoQuimicoPorHectarea { get; set; }
    public double PrecioPera { get; set; }
    public bool AplicarQuimicos { get; set; }
    public bool AplicarFeromonas { get; set; }
}
