using System.Collections.Generic;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Metadata.Itens.Armas.UmaMaoArmas
{
    public class Varinhas
    {
        public List<RPArmaVarinha> VarinhasAb()
        {
            var varinha = new List<RPArmaVarinha>();
            varinha.Add(Varinha1());
            varinha.Add(Varinha2());
            varinha.Add(Varinha3());
            return varinha;
        }

        public RPArmaVarinha Varinha1()
        => new RPArmaVarinha(1, "Varinha de Madeira Balsa", RPClasse.UmaMao, 3, new RPDano(5, 9), 0.07, 1.4, 14);

        public RPArmaVarinha Varinha2()
        => new RPArmaVarinha(6, "Chifre de Cabra", RPClasse.UmaMao, 3, new RPDano(9, 16), 0.07, 1.2, 29);

        public RPArmaVarinha Varinha3()
        => new RPArmaVarinha(12, "Varinha Gravada", RPClasse.UmaMao, 3, new RPDano(9, 17), 0.07, 1.5, 47);
    }
}
