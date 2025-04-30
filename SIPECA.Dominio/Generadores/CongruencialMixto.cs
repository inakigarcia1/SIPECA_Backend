using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneradoresYPruebas.Dominio.Generadores;

namespace GeneradoresYPruebas.Generadores;
public class CongruencialMixto
{
    public static List<double> ObtenerNumerosAleatorios(double semilla, double a, double c, double m, double cantidadDeNumerosAGenerar)
    {
        var numerosAleatorios = new List<double>();

        for (double i = 0; i < cantidadDeNumerosAGenerar; i++)
        {
            semilla = (double) ObtenerSiguienteResto(semilla, a, c, m);
            numerosAleatorios.Add(Math.Round((double)semilla / m, 6));
        }

        return numerosAleatorios;
    }

    private static double ObtenerSiguienteResto(double restoPrevio, double a, double c, double m)
    {
        return (a * restoPrevio + c) % m;
    }
}
