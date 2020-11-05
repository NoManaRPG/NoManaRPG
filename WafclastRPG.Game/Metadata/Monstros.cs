using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Metadata.Itens;

namespace WafclastRPG.Game.Metadata
{
    public static class Monstros
    {
        public static WafclastMonstro VacaAb()
            => new WafclastMonstro("Vaca", Comida.CarneCruAb());
    }
}
