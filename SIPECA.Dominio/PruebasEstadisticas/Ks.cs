using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Linq;

namespace SIPECA.Dominio.PruebasEstadisticas
{
    public static class Ks
    {
        public static (bool esAleatorio, double estadistico) EsAleatorio(double valorCritico, double[] numeros)
        {
            int n = numeros.Length;

            // 1. Ordenar los números
            var ordenados = numeros.OrderBy(x => x).ToArray();

            // 2. Calcular Dn = Máx|Fn(xi) - ui|
            double maxD = 0.0;

            for (int i = 0; i < n; i++)
            {
                double FnXi = (i + 1.0) / n;
                double diferencia = Math.Abs(FnXi - ordenados[i]);
                if (diferencia > maxD)
                    maxD = diferencia;
            }

            // 3. Comparar con valor crítico
            bool esAleatorio = maxD < valorCritico;

            return (esAleatorio, maxD);
        }
    }
}

