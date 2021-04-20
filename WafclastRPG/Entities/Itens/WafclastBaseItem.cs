using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(WafclastLevelItem), typeof(WafclastMonsterCoreItem), typeof(WafclastOreItem),
    typeof(WafclastPickaxeItem), typeof(WafclastRawFoodItem), typeof(WafclastCookedFoodItem))]
    public class WafclastBaseItem
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public ulong PlayerId { get; set; }
        public string Name { get; set; }
        public bool CanSell { get; set; } = true;
        public ulong Quantity { get; set; } = 1;
        public string ImageURL { get; set; } = "";
        public string Description { get; set; } = "Sem descrição";

        public WafclastBaseItem() { }

        public WafclastBaseItem(WafclastBaseItem baseItem)
        {
            Id = baseItem.Id;
            PlayerId = baseItem.PlayerId;
            Name = baseItem.Name;
            CanSell = baseItem.CanSell;
            Quantity = baseItem.Quantity;
            ImageURL = baseItem.ImageURL;
            Description = baseItem.Description;
        }
    }
}
