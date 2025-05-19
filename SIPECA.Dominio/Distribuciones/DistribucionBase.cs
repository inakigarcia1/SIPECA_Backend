using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Interfaces;

namespace SIPECA.Dominio.Distribuciones;
public abstract class DistribucionBase
{
    protected IGenerador Generador { get; init; }

    protected DistribucionBase(IGenerador generador)
    {
        Generador = generador;
    }
}
