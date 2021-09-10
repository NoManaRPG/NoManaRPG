using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace WafclastRPG.Entities.Itens {
  public enum StyleType {
    Melee,
    Ranged,
    Magic
  }

  public enum SlotEquipament {
    Head,
    Back,
    Neck,
    Ammunition,
    [Description("Mão primária")]
    MainHand,
    [Description("Mão secundária")]
    OffHand,
    Torso,
    Leg,
    Hands,
    Feet,
    Ring,
    Aura,
  }

  [BsonIgnoreExtraElements]
  public class WafclastEquipableItem : WafclastBaseItem {
    public StyleType Style { get; set; }
    public SlotEquipament Slot { get; set; }
    public int RequiredLevel { get; set; }

    public WafclastEquipableItem() { }
    public WafclastEquipableItem(WafclastBaseItem baseItem) : base(baseItem) { }
  }
}
