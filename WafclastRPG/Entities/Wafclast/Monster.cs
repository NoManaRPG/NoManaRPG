using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using WafclastRPG.Enums;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Entities.Wafclast {
  [BsonIgnoreExtraElements]
  public class Monster {
    public int Level { get; set; }
    public string Name { get; set; }

    public string Mention { get { return $"{Name} [Nv. {Level}]"; } }
    public bool IsDead { get { return LifePoints.Current <= 0; } }

    public WafclastAttributes Attributes { get; set; }
    public WafclastStatePoints LifePoints { get; set; }


    public DamageType DamageType { get; set; }
    public double Damage { get; set; }
    public double EvasionPoints { get; set; }
    public double PrecisionPoints { get; set; }
    public double AttackSpeed { get; set; }

    public List<DropChance> Drops { get; set; } = new List<DropChance>();

    public Monster(int level, string name, DamageType damageType, double strength = 5, double constitution = 5, double dexterity = 5, double agility = 5, double intelligence = 5, double willpower = 5, double perception = 5, double charisma = 5) {
      Level = level;
      Name = name;
      DamageType = damageType;
      Attributes = new WafclastAttributes(strength, constitution, dexterity, intelligence, willpower, perception, charisma);
    }

    public void CalculateStatistics() {
      LifePoints = new WafclastStatePoints(CalculateLifePoints(Attributes));
      EvasionPoints = CalculateEvasionPoints(Attributes);
      PrecisionPoints = CalculateDexteryPoints(Attributes);
      AttackSpeed = CalculateAttackSpeed(Attributes);
      if (DamageType == DamageType.Magic) Damage = CalculateMagicalDamage(Attributes);
      else
        Damage = CalculatePhysicalDamage(Attributes);
    }

    public double TakeDamage(double valor) {
      LifePoints.Remove(valor);
      return valor;
    }
  }

  public class DropChance {
    public int GlobalItemId { get; set; }
    public double Chance { get; set; }
    public int MinQuantity { get; set; }
    public int MaxQuantity { get; set; }

    public DropChance() { }

    public DropChance(int globalItemId, double chance, int minQuantity, int maxQuantity) {
      GlobalItemId = globalItemId;
      Chance = chance;
      MinQuantity = minQuantity;
      MaxQuantity = maxQuantity;
    }
  }
}
