using WafclastRPG.Entities.Itens;

namespace WafclastRPG.DataBases {
  public class DatabaseItems {
    public WafclastWeaponItem BronzeDagger() {
      var weapon = new WafclastWeaponItem() {
        GlobalItemId = 0,
        Name = "Adaga de Bronze",
        Description = "Pequena, mas pontuda.",
        Style = StyleType.Melee,
        Slot = SlotEquipament.MainHand,
        RequiredLevel = 1,
        Damage = 48,
        Accuracy = 150,
        Speed = AttackRate.Fastest,
      };
      return weapon;
    }
  }
}
