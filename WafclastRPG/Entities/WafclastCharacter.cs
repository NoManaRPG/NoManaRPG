using MongoDB.Bson.Serialization.Attributes;
using System;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; } = new WafclastCoins(20);

        public WafclastStatePoints PhysicalDamage { get; set; }
        public WafclastStatePoints Evasion { get; set; }
        public WafclastStatePoints Accuracy { get; set; }
        public WafclastStatePoints Armour { get; set; }
        public WafclastStatePoints EnergyShield { get; set; }

        public WafclastStatePoints Intelligence { get; set; }
        public WafclastStatePoints Dexterity { get; set; }
        public WafclastStatePoints Strength { get; set; }

        public WafclastStatePoints Life { get; set; }
        public WafclastStatePoints Mana { get; set; }

        public WafclastStatePoints LifeRegen { get; set; }
        public WafclastStatePoints ManaRegen { get; set; }

        public ulong AttributePoints { get; set; }
        public int CurrentFloor { get; set; }
        public WafclastMonster Monster { get; set; }
        public WafclastPickaxeItem Pickaxe { get; set; }

        /// <summary>
        /// Determina a chance de cair mais de 1 minério.
        /// </summary>
        public WafclastLevel MineSkill { get; set; }

        /// <summary>
        /// Determina a chance de cozinhar e quais pode.
        /// </summary>
        public WafclastLevel CookingSkill { get; set; }

        public DateTime RegenDate { get; set; } = DateTime.UtcNow;

        public WafclastCharacter()
        {
            Strength = new WafclastStatePoints(20);
            Intelligence = new WafclastStatePoints(20);
            Dexterity = new WafclastStatePoints(20);

            PhysicalDamage = new WafclastStatePoints(8);
            PhysicalDamage.MultValue += Strength.CurrentValue * 0.2;
            PhysicalDamage.Restart();

            Evasion = new WafclastStatePoints(53);
            Evasion.MultValue += Dexterity.CurrentValue * 0.2;
            Evasion.Restart();

            Accuracy = new WafclastStatePoints(Dexterity.CurrentValue * 2);

            Armour = new WafclastStatePoints(0);

            EnergyShield = new WafclastStatePoints(0);
            EnergyShield.MultValue += Intelligence.CurrentValue * 0.2;

            Life = new WafclastStatePoints(50);
            Life.BaseValue += Strength.CurrentValue * 0.5;
            Life.Restart();

            Mana = new WafclastStatePoints(40);
            Mana.BaseValue += Intelligence.CurrentValue * 0.5;
            Mana.Restart();

            LifeRegen = new WafclastStatePoints(0);
            ManaRegen = new WafclastStatePoints(Mana.MaxValue * 0.08);

            MineSkill = new WafclastLevel(1);
            CookingSkill = new WafclastLevel(1);
        }

        public double DodgeChance(double attackerkAccuracy)
        {
            var attack = 1.15 * attackerkAccuracy;
            var def = Evasion.CurrentValue * 0.25;
            var div = Math.Pow(Convert.ToDouble(attack / attackerkAccuracy + def), 0.8);
            div = Math.Clamp(div, 0, 95);
            return div / 100;
        }

        public double HitChance(double defenderEvasion)
        {
            var attack = 1.15 * Accuracy.CurrentValue;
            var def = defenderEvasion * 0.25;
            var div = Math.Pow(Convert.ToDouble(attack / Accuracy.CurrentValue + def), 0.8);
            div = Math.Clamp(div, 0.5, 100);
            return div / 100;
        }

        public double DamageReduction(double damage)
        {
            var first = Armour.CurrentValue * damage;
            var second = (Armour.CurrentValue + 10) * damage;
            var dr = first / second;
            return damage - damage * dr;
        }

        public new bool AddExperience(double exp)
        {
            int levelUps = base.AddExperience(exp);
            for (int i = 0; i < levelUps; i++)
            {
                Life.BaseValue += 12;
                Accuracy.BaseValue += 2;
                if (Level > BlockedLevel)
                    AttributePoints += 10;
            }

            if (levelUps >= 1)
            {
                Accuracy.Restart();
                Life.Restart();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retorna true caso tenha sido eliminado.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool ReceiveDamage(double valor)
        {
            if (Life.Remove(valor))
            {
                if (Level != 1)
                {
                    Life.BaseValue -= 12;
                    Accuracy.BaseValue -= 2;
                    RemoveOneLevel();
                }
                Life.Restart();
                return true;
            }
            return false;
        }
    }
}
