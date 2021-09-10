using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens {
  [BsonIgnoreExtraElements]
  [BsonDiscriminator(RootClass = true)]
  [BsonKnownTypes(typeof(WafclastCookedFoodItem), typeof(WafclastEquipableItem),
      typeof(WafclastWeaponItem))]
  public class WafclastBaseItem {
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

    public WafclastBaseItem(WafclastBaseItem baseItem) {
      Id = baseItem.Id;
      PlayerId = baseItem.PlayerId;
      GlobalItemId = baseItem.GlobalItemId;
      Name = baseItem.Name;
      CanSell = baseItem.CanSell;
      CanStack = baseItem.CanStack;
      Quantity = baseItem.Quantity;
      ImageURL = baseItem.ImageURL;
      Description = baseItem.Description;
    }
  }
}
