using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradoresYPruebas.Dominio.Generadores;
public class CongruencialAditivo
{
    public static List<double> ObtenerNumerosAleatorios(double m, double cantidadNumerosAGenerar, params double[] valoresN)
    {
        var numerosAleatorios = new List<double>();
        double k = (double) valoresN.Length - 1;

        var diccionarioN = new SortedDictionary<double, double>();

        for(int i = -(valoresN.Length - 1); i < 1; i++)
        {
            diccionarioN.Add(i, valoresN[Math.Abs(i)]);
        }

        double resto = diccionarioN[0];
        for (double i = 0; i < cantidadNumerosAGenerar; i++)
        {
            resto = (double)ObtenerSiguienteResto(resto, m, k, i, diccionarioN);
            diccionarioN.Add((double)i + 1, resto);
            numerosAleatorios.Add(Math.Round((double)resto / m, 6));
        }

        return numerosAleatorios;
    }

    private static double ObtenerSiguienteResto(double restoPrevio, double m, double k, double i, SortedDictionary<double, double> diccionarioN)
    {
        return (restoPrevio + diccionarioN[(double)i - (double)k]) % m;
    }
}
