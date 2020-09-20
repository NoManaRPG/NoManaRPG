using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class Adagas
    {
        public List<RPAdaga> AdagasAb()
        {
            var arma = new List<RPAdaga>();
            arma.Add(Adaga1());
            arma.Add(Adaga2());
            arma.Add(Adaga3());
            return arma;
        }

        public RPAdaga Adaga1()
        => new RPAdaga(1, "Faca de Vidro", RPClasse.UmaMaoArma, 3, new RPDano(6, 10), 0.06, 1.5, 9, 6);

        public RPAdaga Adaga2()
        => new RPAdaga(5, "Faca de Esfola", RPClasse.UmaMaoArma, 3, new RPDano(4, 17), 0.06, 1.45, 16, 11);

        public RPAdaga Adaga3()
        => new RPAdaga(15, "Estilete", RPClasse.UmaMaoArma, 3, new RPDano(7, 27), 0.061, 1.5, 30, 30);

    }
}
