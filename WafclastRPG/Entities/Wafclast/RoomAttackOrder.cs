using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Wafclast {
  [BsonIgnoreExtraElements]
  public class RoomAttackOrder {
    public double PlayerAttackSpeedPoints { get; set; }
    public double MonsterAttackSpeedPoints { get; set; }
    public double TotalAttackSpeedPoints { get; set; }

    public (bool isPlayer, bool isMonster) CalculateNextAttack(double playerAttackSpeed, double monsterAttackSpeed) {
      bool isPlayerAttacking = false;
      bool isMonsterAttacking = false;

      while (isPlayerAttacking == false || isMonsterAttacking == false) {
        PlayerAttackSpeedPoints += playerAttackSpeed;
        MonsterAttackSpeedPoints += monsterAttackSpeed;

        if (PlayerAttackSpeedPoints / TotalAttackSpeedPoints >= 1) {
          PlayerAttackSpeedPoints -= TotalAttackSpeedPoints;
          isPlayerAttacking = true;
        }

        if (MonsterAttackSpeedPoints / TotalAttackSpeedPoints >= 1) {
          MonsterAttackSpeedPoints -= TotalAttackSpeedPoints;
          isMonsterAttacking = true;
        }
      }

      return (isPlayerAttacking, isMonsterAttacking);
    }
  }
}
