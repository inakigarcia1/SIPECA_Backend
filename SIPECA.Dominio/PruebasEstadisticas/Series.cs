using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPECA.Dominio.PruebasEstadisticas
{
    public class Series
    {
        public static (bool esAleatorio, double estadistico) EsAleatorio(double x, double comparador, double n, params double[] valoresU)
        {
            int xCuadrado = (int)(x * x);
            double frecuenciaEsperada = n / xCuadrado;

            var frecuencias = new Dictionary<(int, int), int>();

            for (int i = 0; i < n; i++)
            {
                double u1 = valoresU[2 * i];
                double u2 = valoresU[2 * i + 1];

                int j = (int)(u1 * x);
                int k = (int)(u2 * x);

                var celda = (j, k);

                if (!frecuencias.ContainsKey(celda))
                    frecuencias[celda] = 0;

                frecuencias[celda]++;
            }

            double sumatoria = 0.0;
            foreach (var frecuencia in frecuencias.Values)
            {
                sumatoria += Math.Pow(frecuencia - frecuenciaEsperada, 2);
            }

            double chiCuadrado = xCuadrado / (double)n * sumatoria;

            return (chiCuadrado < comparador, chiCuadrado);
        }
    }

}
