using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Generadores;

namespace SIPECA.Dominio.Distribuciones.Continuas;
public class Uniforme(IGenerador generador) : DistribucionBase(generador)
{
    public double GenerarVariableAleatoria(double a, double b, bool truncar = true)
    {
        return truncar? Math.Truncate(a + (b - a) * Generador.GenerarU()) : a + (b - a) * Generador.GenerarU();
    }
}
