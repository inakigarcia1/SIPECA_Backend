using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Interfaces;

namespace SIPECA.Dominio.Distribuciones.Discretas;
public class Poisson(IGenerador generador) : DistribucionBase(generador)
{
    public double GenerarVariableAleatoria(double cantidadEventosPorContinuo)
    {
        var b = Math.Exp(-cantidadEventosPorContinuo);
        double x = 0;
        double p = 1;
        while(p > b)
        {
            p *= Generador.GenerarU();
            x++;
        }
        return x;
    }
}
