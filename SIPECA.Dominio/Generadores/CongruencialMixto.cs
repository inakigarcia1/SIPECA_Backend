using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Interfaces;

namespace SIPECA.Dominio.Generadores;
public class CongruencialMixto : IGenerador
{
    public double Semilla { get; private set; } = 12346d;
    public double A { get; init; } = 1103515245d;
    public double C { get; init; } = 12345d;
    public double M { get; init; } = 2147483648d;

    public CongruencialMixto(double semilla, double a, double c, double m)
    {
        Semilla = semilla;
        A = a;
        C = c;
        M = m;
    }

    public CongruencialMixto() { }

    public double GenerarU()
    {
        Semilla = (A * Semilla + C) % M;
        return Math.Round(Semilla / M, 6);
    }
}
