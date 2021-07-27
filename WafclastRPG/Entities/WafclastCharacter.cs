using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacter
    {
        public double Armour { get; set; } = 0;
        public double Accuracy { get; set; }

        public WafclastStatePoints LifePoints { get; set; }

        public WafclastMonster CurrentFightingMonster { get; set; }

        public List<WafclastBaseItem> Inventory { get; set; } = new List<WafclastBaseItem>();

        #region Skills

        public WafclastLevel AttackSkill { get; set; } = new WafclastLevel();
        public WafclastLevel StrengthSkill { get; set; } = new WafclastLevel();
        public WafclastLevel DefenceSkill { get; set; } = new WafclastLevel();
        public WafclastLevel ConstitutionSkill { get; set; } = new WafclastLevel(10);

        public WafclastLevel MineSkill { get; set; } = new WafclastLevel();
        public WafclastLevel CookingSkill { get; set; } = new WafclastLevel();

        #endregion Skills

        #region Equipment

        public WafclastWeaponItem MainHand { get; set; }
        public WafclastWeaponItem OffHand { get; set; }
        public WafclastWeaponItem TwoHanded { get; set; }

        #endregion

        public int RegionId { get; set; } = 0;


        public WafclastCharacter()
        {
            LifePoints = new WafclastStatePoints(ConstitutionSkill.Level * 100);
            Accuracy = CalculateAccuracy(0);
            AddItem(new DatabaseItems().BronzeDagger());
        }

        public bool ReceiveDamage(double value) => LifePoints.Remove(value);

        public bool AddItem(WafclastBaseItem baseItem)
        {
            if (baseItem.CanStack)
            {
                foreach (var item in Inventory)
                {
                    if (item.GlobalItemId == baseItem.GlobalItemId)
                    {
                        item.Quantity += baseItem.Quantity;
                        return true;
                    }
                }

                if (Inventory.Count == 19)
                    return false;

                Inventory.Add(baseItem);
                return true;
            }

            if (Inventory.Count == 19)
                return false;

            Inventory.Add(baseItem);
            return true;
        }

        public double CalculateMainHandDamage()
        {
            double weaponDamage = 0;
            double weaponSpeed = 1;
            if (MainHand != null)
            {
                weaponDamage = MainHand.Damage;
                switch (MainHand.Speed)
                {
                    case AttackRate.Average:
                        weaponSpeed = 96 / 149;
                        break;
                    case AttackRate.Fast:
                        weaponSpeed = 192 / 245;
                        break;
                }
            }
            return (2.5 * StrengthSkill.Level) + (weaponDamage * weaponSpeed);
        }

        public double CalculateOffHandDamage()
        {
            double weaponDamage = 0;
            double weaponSpeed = 1;
            if (OffHand != null)
            {
                weaponDamage = OffHand.Damage;
                switch (OffHand.Speed)
                {
                    case AttackRate.Average:
                        weaponSpeed = 96 / 149;
                        break;
                    case AttackRate.Fast:
                        weaponSpeed = 192 / 245;
                        break;
                }
            }
            return (1.25 * StrengthSkill.Level) + (weaponDamage * weaponSpeed);
        }

        public double CalculateTwoHandedDamage()
        {
            double weaponDamage = 0;
            double weaponSpeed = 1;
            if (TwoHanded != null)
            {
                weaponDamage = TwoHanded.Damage;
                switch (TwoHanded.Speed)
                {
                    case AttackRate.Average:
                        weaponSpeed = 96 / 149;
                        break;
                    case AttackRate.Fast:
                        weaponSpeed = 192 / 245;
                        break;
                }
            }
            return (3.75 * StrengthSkill.Level) + (weaponDamage * weaponSpeed);
        }

        public double CalculateHitChance(double armor) => Accuracy / armor;

        public double CalculateAccuracy(double weaponAccuracy)
        {
            return (0.0008 * Math.Pow(AttackSkill.Level, 3)) + (4 * AttackSkill.Level) + 40 + weaponAccuracy;
        }

        //public double CalculateArmor()
        //{

        //}
    }
}
