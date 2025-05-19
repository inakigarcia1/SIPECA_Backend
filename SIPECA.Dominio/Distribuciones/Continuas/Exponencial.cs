using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Generadores;

namespace SIPECA.Dominio.Distribuciones.Continuas;
public class Exponencial(IGenerador generador) : DistribucionBase(generador)
{
    public double GenerarVariableAleatoria(double cantidad, double tiempo)
    {
        var alpha = cantidad / tiempo;
        var esperanza = 1 / alpha;
        return - esperanza * Math.Log(Generador.GenerarU());
    }
}
