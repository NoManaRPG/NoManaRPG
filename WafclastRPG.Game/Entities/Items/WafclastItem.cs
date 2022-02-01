// This file is part of WafclastRPG project.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Items
{
    [BsonIgnoreExtraElements]
    public class WafclastItem : WafclastBaseItem
    {
        // Way to find the item
        [BsonId]
        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
        public ulong PlayerId { get; set; }

        public bool IsOnInventory { get; set; }

        public WafclastItem() { }

        public WafclastItem(WafclastItem item)
        {
            this.ItemId = item.ItemId;
            this.Name = item.Name;
            this.Quantity = item.Quantity;
            this.Volume = item.Volume;
            this.Id = item.Id;
            this.PlayerId = item.PlayerId;
            this.IsOnInventory = item.IsOnInventory;
        }
    }
}
