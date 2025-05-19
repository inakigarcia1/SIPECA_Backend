using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Interfaces;

namespace SIPECA.Dominio.Distribuciones.Continuas;
public class Normal(IGenerador generador) : DistribucionBase(generador)
{
    public double GenerarVariableAleatoria(double media, double desviacion)
    {
        double suma = 0.0;

        for (int i = 1; i <= 12; i++)
        {
            suma += Generador.GenerarU();
        }

        double z = suma - 6.0;
        return desviacion * z + media;
    }

}
