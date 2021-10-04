// This file is part of the WafclastRPG project.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Itens
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(WafclastCookedFoodItem),
                    typeof(WafclastEquipableItem),
                    typeof(WafclastWeaponItem))]
    public class WafclastBaseItem
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public ulong PlayerId { get; set; }
        public int GlobalItemId { get; set; }
        public string Name { get; set; }
        public bool CanSell { get; set; } = true;
        public bool CanStack { get; set; } = false;

        /// <summary>
        /// It's only used for Bank. Not for Inventory, since it cant stack in your inventory.
        /// </summary>
        public ulong Quantity { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }

        public WafclastBaseItem() { }

        public WafclastBaseItem(WafclastBaseItem baseItem)
        {
            this.Id = baseItem.Id;
            this.PlayerId = baseItem.PlayerId;
            this.GlobalItemId = baseItem.GlobalItemId;
            this.Name = baseItem.Name;
            this.CanSell = baseItem.CanSell;
            this.CanStack = baseItem.CanStack;
            this.Quantity = baseItem.Quantity;
            this.ImageURL = baseItem.ImageURL;
            this.Description = baseItem.Description;
        }
    }
}
