using TorreRPG.Entidades.Itens;
using System.Collections.Generic;
using System.Linq;
using TorreRPG.Metadata.Itens.Frascos;
using TorreRPG.Metadata.Itens.Armas.UmaMaoArmas;
using TorreRPG.Metadata.Itens.Armas.DuasMaoArmas;

namespace TorreRPG.BancoItens
{
    public static class RPBancoItens
    {
        public static IEnumerable<IGrouping<int, RPBaseItem>> Itens;

        public static void Carregar()
        {
            var i = new List<RPBaseItem>();
            i.AddRange(new MacasUmaMao().MacasUmaMaoAb());
            i.AddRange(new Arcos().ArcosAb());
            i.AddRange(new MachadosUmaMao().MachadosUmaMaoAb());
            i.AddRange(new Adagas().AdagaAb());
            i.AddRange(new EspadasUmaMao().EspadaAb());
            i.AddRange(new FrascosVida().FrascosVidaAb());

            Itens = i.GroupBy(x => x.DropLevel);
        }
    }
}
