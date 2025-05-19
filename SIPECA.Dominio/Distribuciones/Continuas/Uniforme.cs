using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Interfaces;

namespace SIPECA.Dominio.Distribuciones.Continuas;
public class Uniforme(IGenerador generador) : DistribucionBase(generador)
{
    public double GenerarVariableAleatoria(double a, double b)
    {
        return a + (b - a) * Generador.GenerarU();
    }
}
