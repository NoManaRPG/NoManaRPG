using MongoDB.Bson;

namespace WafclastRPG.Entities.Monsters
{
    public class ItemChance
    {
        public ObjectId ItemId { get; set; }
        public double Chance { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }
    }
}
