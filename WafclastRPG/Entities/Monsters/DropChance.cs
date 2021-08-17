namespace WafclastRPG.Entities.Monsters
{
    public class DropChance
    {
        public int GlobalItemId { get; set; }
        public double Chance { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }

        public DropChance() { }

        public DropChance(int globalItemId, double chance, int minQuantity, int maxQuantity)
        {
            GlobalItemId = globalItemId;
            Chance = chance;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
        }
    }
}
