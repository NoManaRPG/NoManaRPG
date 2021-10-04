// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Rooms
{
    [BsonIgnoreExtraElements]
    public class WafclastRoomAttackOrder
    {
        public double PlayerAttackSpeedPoints { get; set; }
        public double MonsterAttackSpeedPoints { get; set; }
        public double TotalAttackSpeedPoints { get; set; }

        public (bool isPlayer, bool isMonster) CalculateNextAttack(double playerAttackSpeed, double monsterAttackSpeed)
        {
            bool isPlayerAttacking = false;
            bool isMonsterAttacking = false;

            while (isPlayerAttacking == false || isMonsterAttacking == false)
            {
                this.PlayerAttackSpeedPoints += playerAttackSpeed;
                this.MonsterAttackSpeedPoints += monsterAttackSpeed;

                if (this.PlayerAttackSpeedPoints / this.TotalAttackSpeedPoints >= 1)
                {
                    this.PlayerAttackSpeedPoints -= this.TotalAttackSpeedPoints;
                    isPlayerAttacking = true;
                }

                if (this.MonsterAttackSpeedPoints / this.TotalAttackSpeedPoints >= 1)
                {
                    this.MonsterAttackSpeedPoints -= this.TotalAttackSpeedPoints;
                    isMonsterAttacking = true;
                }
            }

            return (isPlayerAttacking, isMonsterAttacking);
        }
    }
}
