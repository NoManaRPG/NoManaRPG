using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens {
  [BsonIgnoreExtraElements]
  public class WafclastCookedFoodItem : WafclastBaseItem {
    /// <summary>
    /// Quantos ganha de vida ao comer o item.
    /// </summary>
    public double LifeGain { get; set; }

    public WafclastCookedFoodItem(WafclastBaseItem baseItem) : base(baseItem) { }
  }
}
