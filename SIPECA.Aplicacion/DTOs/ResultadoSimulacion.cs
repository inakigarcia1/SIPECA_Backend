namespace SIPECA.Aplicacion.DTOs;

public class ResultadoSimulacion
{
    public double CantidadHectareasSanas {  get; set; }
    public double CantidadHectareasInfectadas { get; set; }
    public double PerasSanas { get; set; }
    public double PerasInfectadas { get; set; }
    public double DiasTranscurridos {  get; set; }

    public override string ToString()
    {
        return $"Hectáreas sanas: {CantidadHectareasSanas}, \n" +
               $"Hectáreas infectadas: {CantidadHectareasInfectadas}, \n" +
               $"Peras sanas: {PerasSanas}, \n" +
               $"Peras infectadas: {PerasInfectadas}, \n" +
               $"Días transcurridos: {DiasTranscurridos}\n";
    }
}
