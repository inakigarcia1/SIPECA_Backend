using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Generadores;

namespace SIPECA.Dominio.Distribuciones.Continuas;
public class Normal(IGenerador generador) : DistribucionBase(generador)
{
    public double GenerarVariableAleatoria(double media, double desviacion)
    {
        var suma = 0.0;

        for (var i = 1; i <= 12; i++)
        {
            suma += Generador.GenerarU();
        }

        var z = suma - 6.0;
        return desviacion * z + media;
    }

}
