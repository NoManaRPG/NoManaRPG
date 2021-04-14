using MongoDB.Bson;

namespace WafclastRPG.Entities.Monsters
{
    public class DropChance
    {
        public ObjectId Id { get; set; }
        public double Chance { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }

        public DropChance(ObjectId itemId, double chance, int minQuantity, int maxQuantity)
        {
            Id = itemId;
            Chance = chance;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
        }
    }
}
