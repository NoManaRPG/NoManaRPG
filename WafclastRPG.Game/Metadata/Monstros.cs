using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Metadata.Itens;

namespace WafclastRPG.Game.Metadata
{
    public sealed class Monstros
    {
        public static WafclastMonstro VacaAb() => new WafclastMonstro("Vaca", Comidas.CarneCruAb());
        public static WafclastMonstro SlimeAb() => new WafclastMonstro("Slime", Bebidas.GosmaAb());
        public static WafclastMonstro BandidoAb() => new WafclastMonstro("Bandido", Frascos.FrascoVidaPequenoAb());
    }
}
