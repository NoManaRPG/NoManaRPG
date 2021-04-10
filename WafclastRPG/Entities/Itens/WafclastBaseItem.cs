using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using WafclastRPG.Enums;

namespace WafclastRPG.Entities.Itens
{
    public class WafclastBaseItem
    {
        public ObjectId Id { get; set; }
        public ulong ItemID { get; set; }
        public ulong PlayerId { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int Level { get; set; } = 1;
        public int Price { get; set; } = 0;
        public bool CanSell { get; set; } = true;
        public bool CanStack { get; set; } = true;
        public int Quantity { get; set; } = 1;
        public string ImageURL { get; set; } = "";
        public string Description { get; set; } = "Sem descrição";

        public WafclastBaseItem()
        {
        }

        public WafclastBaseItem(WafclastBaseItem baseItem)
        {
            Id = baseItem.Id;
            ItemID = baseItem.ItemID;
            PlayerId = baseItem.PlayerId;
            Name = baseItem.Name;
            Type = baseItem.Type;
            Level = baseItem.Level;
            Price = baseItem.Price;
            CanSell = baseItem.CanSell;
            CanStack = baseItem.CanStack;
            Quantity = baseItem.Quantity;
            ImageURL = baseItem.ImageURL;
            Description = baseItem.Description;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastBaseItem>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
                cm.SetIsRootClass(true);
            });
            BsonClassMap.RegisterClassMap<WafclastFood>();
            BsonClassMap.RegisterClassMap<WafclastMonsterCore>();
        }
    }
}
