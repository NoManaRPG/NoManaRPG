// This file is part of the WafclastRPG project.

namespace WafclastRPG.Game.Entities.Monsters
{
    public class WafclastMonsterReward
    {
        public int GlobalItemId { get; set; }
        public double Chance { get; set; }
        public int QuantityMin { get; set; }
        public int QuantityMax { get; set; }

        public WafclastMonsterReward(int globalItemId, double chance, int quantityMin, int quantityMax)
        {
            this.GlobalItemId = globalItemId;
            this.Chance = chance / 100;
            this.QuantityMin = quantityMin;
            this.QuantityMax = quantityMax;
        }
    }
}
