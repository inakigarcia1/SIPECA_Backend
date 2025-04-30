using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace GeneradoresYPruebas.Dominio.Pruebas;
public class Frecuencia
{
    public static (bool esAleatorio, double estadistico) EsAleatorio(double x, double comparador, params double[] valoresU)
    {
        var n = valoresU.Length;
        var listaU = new List<double>(valoresU).Order().ToList();
        var tamañodoubleervalo = 1 / (double)x;
        var frecuenciaEsperada = n / (double)x;

        var frecuenciasObservadas = new Dictionary<double, double>();
        double numerodoubleervalo = 1;

        for (double i = 0; i <= 1; i += tamañodoubleervalo)
        {
            for (int j = 0; j < listaU.Count; j++)
            {
                if (listaU[j] < i || listaU[j] > i + tamañodoubleervalo) continue;
                if (frecuenciasObservadas.TryAdd(numerodoubleervalo, 1)) continue;

                frecuenciasObservadas[numerodoubleervalo]++;
            }
            numerodoubleervalo++;
        }

        double sumatoria = 0;
        foreach (var observacion in frecuenciasObservadas)
        {
            sumatoria += Math.Pow(observacion.Value - frecuenciaEsperada, 2);
        }

        var chiCuadrado = ((double)x / n) * sumatoria;

        return (chiCuadrado < comparador, chiCuadrado);
    }
}
