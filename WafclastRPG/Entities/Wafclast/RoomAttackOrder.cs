using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Wafclast {
  [BsonIgnoreExtraElements]
  public class RoomAttackOrder {
    public double PlayerAttackSpeedPoints { get; set; }
    public double MonsterAttackSpeedPoints { get; set; }
    public double TotalAttackSpeedPoints { get; set; }

    public static (bool isPlayer, bool isMonster) CalculateNextAttack(BaseCharacter character) {
      bool isPlayerAttacking = false;
      bool isMonsterAttacking = false;

      while (isPlayerAttacking == false || isMonsterAttacking == false) {
        character.Room.AttackOrder.PlayerAttackSpeedPoints += character.AttackSpeed;
        character.Room.AttackOrder.MonsterAttackSpeedPoints += character.Room.Monster.AttackSpeed;

        if (character.Room.AttackOrder.PlayerAttackSpeedPoints / character.Room.AttackOrder.TotalAttackSpeedPoints >= 1) {
          character.Room.AttackOrder.PlayerAttackSpeedPoints -= character.Room.AttackOrder.TotalAttackSpeedPoints;
          isPlayerAttacking = true;
        }

        if (character.Room.AttackOrder.MonsterAttackSpeedPoints / character.Room.AttackOrder.TotalAttackSpeedPoints >= 1) {
          character.Room.AttackOrder.MonsterAttackSpeedPoints -= character.Room.AttackOrder.TotalAttackSpeedPoints;
          isMonsterAttacking = true;
        }
      }

      return (isPlayerAttacking, isMonsterAttacking);
    }
  }
}
