using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Interfaces;

namespace SIPECA.Dominio.Distribuciones.Discretas;
public class Binomial(IGenerador generador) : DistribucionBase(generador)
{
    public void RealizarProcedimientoBinomial(List<Tuple<double, Action>> alternativas)
    {
        var acumuladas = CrearAcumuladas(alternativas);


        var u = Generador.GenerarU();


    }

    private List<Tuple<double, Action>> CrearAcumuladas(List<Tuple<double, Action>> alternativas)
    {
        var acumuladas = new List<double>();
        acumuladas.Add(alternativas[0].Item1);

        for (int i = 1; i < alternativas.Count; i++)
        {
            acumuladas.Add(alternativas[i].Item1 + acumuladas[i - 1]);
        }

        var acumuladasFinal = new List<Tuple<double, Action>>();
        for (int i = 0; i < alternativas.Count; i++)
        {
            acumuladasFinal.Add(new Tuple<double, Action>(acumuladas[i], alternativas[i].Item2));
        }

        return acumuladasFinal;
    }
}
