using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIPECA.Dominio.Generadores;

namespace SIPECA.Dominio.Distribuciones.Discretas;
public class Binomial(IGenerador generador, List<Tuple<double, Action>> alternativas)
    : DistribucionBase(generador)
{
    public List<Tuple<double, Action>> Acumuladas { get; init; } = CrearAcumuladas(alternativas);

    public void RealizarProcedimientoBinomial()
    {
        var u = Generador.GenerarU();
        Acumuladas.First(a => a.Item1 <= u).Item2.Invoke();
    }

    private static List<Tuple<double, Action>> CrearAcumuladas(List<Tuple<double, Action>> alternativas)
    {
        var acumuladas = new List<double>
        {
            alternativas[0].Item1
        };

        for (var i = 1; i < alternativas.Count; i++)
        {
            acumuladas.Add(alternativas[i].Item1 + acumuladas[i - 1]);
        }

        return alternativas.Select((t, i) => new Tuple<double, Action>(acumuladas[i], t.Item2)).ToList();
    }
}
