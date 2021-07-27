using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WafclastRPG.Entities.Monsters
{
    [BsonIgnoreExtraElements]
    public class WafclastMonster
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public double LifePoints { get; set; }

        public double MaxDamage { get; set; }

        public int ArmorTotal { get; set; }
        public int AccuracyTotal { get; set; }

        /// <summary>
        /// Ticks
        /// </summary>
        public double AttackRate { get; set; }

        public List<DropChance> Drops { get; set; } = new List<DropChance>();

        /// <summary>
        /// Retorna true caso tenha sido abatido.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool TakeDamage(double valor)
        {
            LifePoints -= valor;
            if (LifePoints <= 0)
                return true;
            return false;
        }

        public double CalculateHitChance(double armor)
        {
            if (armor == 0)
                return 0;
            return AccuracyTotal / armor;
        }
    }
}
