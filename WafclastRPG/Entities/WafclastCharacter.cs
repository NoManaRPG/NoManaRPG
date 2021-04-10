using MongoDB.Bson.Serialization;
using System;

namespace WafclastRPG.Entities
{
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; } = new WafclastCoins(20);
        #region Localization
        public WafclastLocalization Localization { get; set; }
        public WafclastLocalization LocalizationSpawnPoint { get; set; }
        #endregion

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


        public int AttributePoints = 0;
        public int Karma { get; set; } = 0;

        public WafclastInventory Inventory { get; set; } = new WafclastInventory();
        public DateTime RegenDate { get; set; } = DateTime.UtcNow;

        public WafclastCharacter()
        {
            Strength = new WafclastStatePoints(20);
            Intelligence = new WafclastStatePoints(20);
            Dexterity = new WafclastStatePoints(20);

            PhysicalDamage = new WafclastStatePoints(8);
            PhysicalDamage.MultValue += Strength.CurrentValue * 0.2M;
            PhysicalDamage.Restart();

            Evasion = new WafclastStatePoints(53);
            Evasion.MultValue += Dexterity.CurrentValue * 0.2M;
            Evasion.Restart();

            Accuracy = new WafclastStatePoints(Dexterity.CurrentValue * 2);

            Armour = new WafclastStatePoints(0);

            EnergyShield = new WafclastStatePoints(0);
            EnergyShield.MultValue += Intelligence.CurrentValue * 0.2M;

            Life = new WafclastStatePoints(50);
            Life.BaseValue += Strength.CurrentValue * 0.5M;
            Life.Restart();

            Mana = new WafclastStatePoints(40);
            Mana.BaseValue += Intelligence.CurrentValue * 0.5M;
            Mana.Restart();

            LifeRegen = new WafclastStatePoints(0);
            ManaRegen = new WafclastStatePoints(Mana.MaxValue * 0.08M);
        }

        /// <summary>
        /// Retorna false case acerte. True caso não acerte.
        /// </summary>
        /// <param name="defenderEvasion"></param>
        /// <returns></returns>
        public double DodgeChance(decimal attackerkAccuracy)
        {
            var attack = 1.15M * attackerkAccuracy;
            var def = Evasion.CurrentValue * 0.25M;
            var div = Math.Pow(Convert.ToDouble(attack / attackerkAccuracy + def), 0.8);
            div = Math.Clamp(div, 0, 95);
            return div / 100;
        }

        /// <summary>
        /// Retorna false case acerte. True caso não acerte.
        /// </summary>
        /// <param name="defenderEvasion"></param>
        /// <returns></returns>
        public double HitChance(decimal defenderEvasion)
        {
            var attack = 1.15M * Accuracy.CurrentValue;
            var def = defenderEvasion * 0.25M;
            var div = Math.Pow(Convert.ToDouble(attack / Accuracy.CurrentValue + def), 0.8);
            div = Math.Clamp(div, 0.5, 100);
            return div / 100;
        }

        public decimal DamageReduction(decimal damage)
        {
            var first = Armour.CurrentValue * damage;
            var second = (Armour.CurrentValue + 10) * damage;
            var dr = first / second;
            return damage - damage * dr;
        }

        public new bool AddExperience(decimal exp)
        {
            int levelUps = base.AddExperience(exp);
            for (int i = 0; i < levelUps; i++)
            {
                Life.BaseValue += 12;
                Accuracy.BaseValue += 2;
                if (Level > LevelBloqueado)
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
        public bool ReceiveDamage(decimal valor)
        {
            if (Life.Remove(valor))
            {
                Localization = LocalizationSpawnPoint;
                Karma = 0;

                if (Level != 1)
                {
                    Life.BaseValue -= 12;
                    RemoveOneLevel();
                }
                Life.Restart();
                return true;
            }
            return false;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastCharacter>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
