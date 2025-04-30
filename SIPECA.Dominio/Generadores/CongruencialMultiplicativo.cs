using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradoresYPruebas.Dominio.Generadores;
public class CongruencialMultiplicativo
{
    public static List<double> ObtenerNumerosAleatorios(double semilla, double a, double m, double cantidadDeNumerosAGenerar)
    {
        var numerosAleatorios = new List<double>();

        for (double i = 0; i < cantidadDeNumerosAGenerar; i++)
        {
            semilla = (double)ObtenerSiguienteResto(semilla, a, m);
            numerosAleatorios.Add(Math.Round((double)semilla / m, 6));
        }

        return numerosAleatorios;
    }

    private static double ObtenerSiguienteResto(double restoPrevio, double a, double m)
    {
        return (a * restoPrevio) % m;
    }
}
