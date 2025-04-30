using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradoresYPruebas.Dominio.Pruebas
{
    public static class CorridaDeLaMedia
    {
        public static (bool esAleatorio, double estadistico) EsAleatorio(double chiCuadradoCritico, double[] valores)
        {
            int n = valores.Length;

            // Paso 1 y 2: Secuencia binaria (0 si <= 0.5, 1 si > 0.5)
            var secuenciaBinaria = valores.Select(v => v > 0.5 ? 1 : 0).ToArray();

            // Paso 3: Determinar longitudes de corridas
            var longitudesDeCorrida = CalcularLongitudesDeCorrida(secuenciaBinaria);

            // Agrupar por longitud: frecuencia observada
            var frecuenciasObservadas = longitudesDeCorrida
                .GroupBy(l => l)
                .ToDictionary(g => g.Key, g => g.Count());

            // Paso 4: Calcular frecuencias esperadas (hasta la longitud máxima observada)
            int maxLongitud = longitudesDeCorrida.Max();
            var frecuenciasEsperadas = new Dictionary<int, double>();

            for (int i = 1; i <= maxLongitud; i++)
            {
                frecuenciasEsperadas[i] = (n - i + 3) / Math.Pow(2, i + 1);
            }

            // Paso 5: Calcular estadístico Chi Cuadrado (incluyendo longitudes con frecuencia observada 0)
            double chiCuadrado = 0;

            for (int i = 1; i <= maxLongitud; i++)
            {
                double fo = frecuenciasObservadas.ContainsKey(i) ? frecuenciasObservadas[i] : 0;
                double fe = frecuenciasEsperadas[i];

                chiCuadrado += Math.Pow(fo - fe, 2) / fe;
            }

            // Paso 6: Comparar con el valor crítico
            bool esAleatorio = chiCuadrado < chiCuadradoCritico;
            return (esAleatorio, chiCuadrado);
        }

        private static List<int> CalcularLongitudesDeCorrida(int[] secuencia)
        {
            var longitudes = new List<int>();
            int longitudActual = 1;

            for (int i = 1; i < secuencia.Length; i++)
            {
                if (secuencia[i] == secuencia[i - 1])
                {
                    longitudActual++;
                }
                else
                {
                    longitudes.Add(longitudActual);
                    longitudActual = 1;
                }
            }

            longitudes.Add(longitudActual);
            return longitudes;
        }
    }

}
