using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WafclastRPG.Entities.Monsters
{
    [BsonIgnoreExtraElements]
    public class WafclastMonster
    {
        public int Level { get; set; }
        public string Name { get; set; }

        #region Attributes

        /// <summary>
        /// Força
        /// </summary>
        WafclastAttribute Strength { get; set; }
        /// <summary>
        /// Constituição
        /// </summary>
        WafclastAttribute Constitution { get; set; }
        /// <summary>
        /// Destreza
        /// </summary>
        WafclastAttribute Dexterity { get; set; }
        /// <summary>
        /// Agilidade
        /// </summary>
        WafclastAttribute Agility { get; set; }
        /// <summary>
        /// Inteligencia
        /// </summary>
        WafclastAttribute Intelligence { get; set; }
        /// <summary>
        /// Força de vontade
        /// </summary>
        WafclastAttribute Willpower { get; set; }
        /// <summary>
        /// Percepção
        /// </summary>
        WafclastAttribute Perception { get; set; }

        #endregion

        public WafclastStatePoints LifePoints { get; set; }
        public double Damage { get; set; } = 0;

        public double EvasionPoints { get; set; }
        public double DexteryPoints { get; set; }


        /// <summary>
        /// Attacks
        /// </summary>
        public List<string> Attacks { get; set; }

        public List<DropChance> Drops { get; set; } = new List<DropChance>();

        public WafclastMonster(int level, string name, WafclastAttribute strength, WafclastAttribute constitution, WafclastAttribute dexterity, WafclastAttribute agility, WafclastAttribute intelligence, WafclastAttribute willpower, WafclastAttribute perception)
        {
            Level = level;
            Name = name;
            Strength = strength;
            Constitution = constitution;
            Dexterity = dexterity;
            Agility = agility;
            Intelligence = intelligence;
            Willpower = willpower;
            Perception = perception;

            LifePoints = new WafclastStatePoints(CalculateLifePoints());
            EvasionPoints = CalculateEvasionPoints();
            DexteryPoints = CalculateDexteryPoints();
        }

        public double CalculateLifePoints() => (Constitution * 8.0) + (Strength / 5.0) + ((Constitution / 5.0) * 3.0);

        public double CalculatePhysicalDamage() => Strength + ((Strength / 5.0) * 2.0) + (Dexterity / 2.0);
        public double CalculateMagicalDamage() => Intelligence + ((Intelligence / 5.0) * 2.0) + (Dexterity / 2.0);

        public double CalculateEvasionPoints() => (Agility * 2.0) + (Dexterity / 5.0) + (Intelligence / 5.0) + (Perception / 3.0) + (Willpower / 3.0);
        public double CalculateDexteryPoints() => (Dexterity * 2.0) + (Agility / 5.0) + (Intelligence / 5.0) + (Perception / 3.0) + (Willpower / 3.0);

        public bool ReceiveDamage(double valor) => LifePoints.Remove(valor);


    }
}
