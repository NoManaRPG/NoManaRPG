using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens
{
    public enum AttackRate
    {
        Fastest,
        Fast,
        Average,
        Slow,
        Slowest,
        Random,
    }

    [BsonIgnoreExtraElements]
    public class WafclastWeaponItem : WafclastEquipableItem
    {
        public double Damage { get; set; }
        public double Accuracy { get; set; }
        public AttackRate Speed { get; set; }

        public WafclastWeaponItem() { }
        public WafclastWeaponItem(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
