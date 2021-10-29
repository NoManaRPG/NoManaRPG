// This file is part of the WafclastRPG project.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastItem
    {
        [BsonId]
        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
        public ulong PlayerId { get; set; }
        public int GlobalItemId { get; set; }
        public string Name { get; set; }
        public bool CanSell { get; set; } = false;
        public ulong Quantity { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }

        public WafclastItem() { }

        public WafclastItem(WafclastItem baseItem)
        {
            this.Id = baseItem.Id;
            this.PlayerId = baseItem.PlayerId;
            this.GlobalItemId = baseItem.GlobalItemId;
            this.Name = baseItem.Name;
            this.CanSell = baseItem.CanSell;
            this.Quantity = baseItem.Quantity;
            this.ImageURL = baseItem.ImageURL;
            this.Description = baseItem.Description;
        }
    }
}
