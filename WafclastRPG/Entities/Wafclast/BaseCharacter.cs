using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Entities.Wafclast {
  [BsonIgnoreExtraElements]
  [BsonDiscriminator(RootClass = true)]
  [BsonKnownTypes(typeof(MageCharacter), typeof(CharacterWarrior))]
  public abstract class BaseCharacter : WafclastLevel {

    /// <summary>
    /// All 6 attributes.
    /// </summary>
    public abstract WafclastAttributes Attributes { get; set; }

    /// <summary>
    /// Points to Allocate in Attributes after level up
    /// </summary>
    public int AttributePoints { get; set; } = 5;

    public WafclastStatePoints LifePoints { get; set; }
    public WafclastStatePoints ManaPoints { get; set; }

    public double Damage { get; set; }


    public double EvasionPoints { get; set; }
    public double PrecisionPoints { get; set; }

    public double AttackSpeed { get; set; }

    public abstract string EmojiAttack { get; set; }

    /// <summary>
    /// Name and Level of that skill.
    /// </summary>
    public Dictionary<string, int> Skills = new Dictionary<string, int>();

    public Room Room { get; set; }

    public BaseCharacter() {
      LifePoints = new WafclastStatePoints(CalculateLifePoints(Attributes));
      ManaPoints = new WafclastStatePoints(CalculateManaPoints(Attributes));
      EvasionPoints = CalculateEvasionPoints(Attributes);
      PrecisionPoints = CalculateDexteryPoints(Attributes);
      Damage = CalculateDamagePoints();
      AttackSpeed = CalculateAttackSpeed(Attributes);
    }

    public abstract double CalculateDamagePoints();
    public abstract void ResetCombatThings();

    public (bool isPlayer, bool isMonster) NextAttack() 
      => Room.AttackOrder.CalculateNextAttack(AttackSpeed, Room.Monster.AttackSpeed);

    public double ReceiveDamage(double valor) {
      LifePoints.Remove(valor);
      return valor;
    }

    public bool IsDead {
      get {
        return LifePoints.Current <= 0;
      }
    }
  }
}
