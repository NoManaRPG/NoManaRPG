using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Metadata
{
    public sealed class Regioes
    {
        public static WafclastRegiao Regiao01()
        {
            var reg = new WafclastRegiao(0, RegiaoType.Meizhou, "Fazenda");
            reg.Monstros.Add(Monstros.Vaca1Ab());

            return reg;
        }
    }
}
