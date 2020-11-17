using WafclastRPG.Game.Entidades.NPC;

namespace WafclastRPG.Game.Metadata
{
    public sealed class Monstros
    {
        public static WafclastMonstro Vaca1Ab()
        {
            var monstro = new WafclastMonstro("Vaca", 150, 12, 4, 3, 3);
            var drop1 = new WafclastMonstroDrop(new WafclastMonstroDrop.Raridade(1, 1), 1, 1);
            monstro.Drops.Add(drop1);
            return monstro;
        }
    }
}
