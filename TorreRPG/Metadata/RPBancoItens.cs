using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;
using System.Collections.Generic;
using System.Linq;
using TorreRPG.Metadata.Itens.Armas.DuasMaoArmas.Arcos;
using TorreRPG.Metadata.Itens.Frascos;
using TorreRPG.Metadata.Itens.Armas.UmaMaoArmas;

namespace TorreRPG.BancoItens
{
    public static class RPBancoItens
    {
        public static IEnumerable<IGrouping<int, RPItem>> Itens;

        public static void Carregar()
        {
            var i = new List<RPItem>();
            i.AddRange(new Arcos().ArcosAb());
            i.AddRange(new FrascosVida().FrascosVidaAb());
            i.AddRange(new MachadosUmaMao().MachadosUmaMaoAb());
            i.AddRange(new MacasUmaMao().MacasUmaMaoAb());

            Itens = i.GroupBy(x => x.DropLevel);

            //#region Frascos de vida

            //i.Add(new RPItem(RPClasse.PocoesVida, "Frasco de vida pequeno", 1, 2)
            //{
            //    CargasUso = 7,
            //    CargasMax = 21,
            //    LifeRegen = 70,
            //    Tempo = 6,
            //});
            //i.Add(new RPItem(RPClasse.PocoesVida, "Frasco de vida médio", 3, 2)
            //{
            //    LifeRegen = 150,
            //    Tempo = 6.5,
            //    CargasUso = 8,
            //    CargasMax = 28,
            //});
            //i.Add(new RPItem(RPClasse.PocoesVida, "Frasco de vida grande", 6, 2)
            //{
            //    LifeRegen = 250,
            //    Tempo = 7,
            //    CargasUso = 9,
            //    CargasMax = 30,
            //});

            //#endregion Frascos de vida

        }
    }
}
