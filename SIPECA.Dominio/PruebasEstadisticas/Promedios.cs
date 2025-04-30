using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradoresYPruebas.Dominio.Pruebas;
public class Promedios
{
    public static (bool esAleatorio, double estadistico) EsAleatorio(double comparador, params double[] valoresU)
    {
        var promedio = valoresU.Average();
        var longitud = valoresU.Length;
        var radicando = 1.0 / 12.0;
        var estadistico = ((promedio - 0.5) * Math.Sqrt(longitud)) / Math.Sqrt(radicando);
        return (Math.Abs(estadistico) < comparador, Math.Abs(estadistico));
    }
}
