using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPECA.Aplicacion.DTOs.Response;
public class ResultadosGeneracion
{
    public int Generacion { get; set; }
    public int Dias { get; set; }
    public double HectareasInfectadas { get; set; }
    public double HectareasSanas { get; set; }
    public double PerasSanas { get; set; }
    public double PerasInfectadas { get; set; }
    public double Ganancia { get; set; }
    public double Perdida { get; set; }
    public double CostoTratamientoQuimico { get; set; }
    public double CostoTratamientoFeromonas { get; set; }

}
