// This file is part of the WafclastRPG project.

namespace WafclastRPG.Game.Entities.Monsters
{
    public class WafclastMonsterItemDrop
    {
        public int GlobalItemId { get; set; }
        public double Chance { get; set; }

        public WafclastMonsterItemDrop(int globalItemId, double chance)
        {
            this.GlobalItemId = globalItemId;
            this.Chance = chance;
        }
    }
}
