using System.Collections.Generic;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Armas.UmaMaoArmas
{
    public class EspadaUmaMao
    {
        public List<RPEspada> EspadaAb()
        {
            var arma = new List<RPEspada>();
            arma.Add(Arma1());
            arma.Add(Arma2());
            arma.Add(Arma3());
            arma.Add(Arma4());
            return arma;
        }

        public RPEspada Arma1()
        => new RPEspada(1, "Espada Enferrujada", RPClasse.UmaMaoArma, 3, new RPDano(4, 9), 0.05, 1.55, 8, 8);

        public RPEspada Arma2()
        => new RPEspada(5, "Espada de Cobre", RPClasse.UmaMaoArma, 3, new RPDano(6, 14), 0.05, 1.5, 14, 14);

        public RPEspada Arma3()
        => new RPEspada(10, "Sabre", RPClasse.UmaMaoArma, 3, new RPDano(5, 22), 0.05, 1.55, 18, 26);

        public RPEspada Arma4()
        => new RPEspada(15, "Espada Larga", RPClasse.UmaMaoArma, 3, new RPDano(15, 21), 0.05, 1.45, 30, 30);
    }
}
