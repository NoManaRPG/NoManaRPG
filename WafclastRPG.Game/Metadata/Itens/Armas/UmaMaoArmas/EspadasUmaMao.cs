using System.Collections.Generic;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Metadata.Itens.Armas.UmaMaoArmas
{
    public class EspadasUmaMao
    {
        public List<RPArmaEspada> EspadasAb()
        {
            var arma = new List<RPArmaEspada>();
            arma.Add(Espada1());
            arma.Add(Espada2());
            arma.Add(Espada3());
            arma.Add(Espada4());
            return arma;
        }

        public RPArmaEspada Espada1()
        => new RPArmaEspada(1, "Espada Enferrujada", RPClasse.UmaMao, 3, new RPDano(4, 9), 0.05, 1.55, 8, 8);

        public RPArmaEspada Espada2()
        => new RPArmaEspada(5, "Espada de Cobre", RPClasse.UmaMao, 3, new RPDano(6, 14), 0.05, 1.5, 14, 14);
         
        public RPArmaEspada Espada3()
        => new RPArmaEspada(10, "Sabre", RPClasse.UmaMao, 3, new RPDano(5, 22), 0.05, 1.55, 18, 26);

        public RPArmaEspada Espada4()
        => new RPArmaEspada(15, "Espada Larga", RPClasse.UmaMao, 3, new RPDano(15, 21), 0.05, 1.45, 30, 30);
    }
}
