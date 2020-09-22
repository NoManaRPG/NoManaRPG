using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class Adagas
    {
        public List<RPArmaAdaga> AdagasAb()
        {
            var arma = new List<RPArmaAdaga>();
            arma.Add(Adaga1());
            arma.Add(Adaga2());
            arma.Add(Adaga3());
            return arma;
        }

        public RPArmaAdaga Adaga1()
        => new RPArmaAdaga(1, "Faca de Vidro", RPClasse.UmaMao, 3, new RPDano(6, 10), 0.06, 1.5, 9, 6);

        public RPArmaAdaga Adaga2()
        => new RPArmaAdaga(5, "Faca de Esfola", RPClasse.UmaMao, 3, new RPDano(4, 17), 0.06, 1.45, 16, 11);

        public RPArmaAdaga Adaga3()
        => new RPArmaAdaga(15, "Estilete", RPClasse.UmaMao, 3, new RPDano(7, 27), 0.061, 1.5, 30, 30);

    }
}
