using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public abstract class WafclastCharacter : WafclastLevel
    {

        #region Attributes

        /// <summary>
        /// Força
        /// </summary>
        WafclastAttribute Strength { get; set; } = new WafclastAttribute(5);
        /// <summary>
        /// Constituição
        /// </summary>
        WafclastAttribute Constitution { get; set; } = new WafclastAttribute(5);
        /// <summary>
        /// Destreza
        /// </summary>
        WafclastAttribute Dexterity { get; set; } = new WafclastAttribute(5);
        /// <summary>
        /// Agilidade
        /// </summary>
        WafclastAttribute Agility { get; set; } = new WafclastAttribute(5);
        /// <summary>
        /// Inteligencia
        /// </summary>
        WafclastAttribute Intelligence { get; set; } = new WafclastAttribute(5);
        /// <summary>
        /// Força de vontade
        /// </summary>
        WafclastAttribute Willpower { get; set; } = new WafclastAttribute(5);
        /// <summary>
        /// Percepção
        /// </summary>
        WafclastAttribute Perception { get; set; } = new WafclastAttribute(5);
        /// <summary>
        /// Carisma
        /// </summary>
        WafclastAttribute Charisma { get; set; } = new WafclastAttribute(5);

        #endregion

        public double AttributePoints { get; set; } = 5;

        public WafclastStatePoints LifePoints { get; set; }
        public WafclastStatePoints ManaPoints { get; set; }

        public double Damage { get; set; }
        public double EvasionPoints { get; set; }
        public double DexteryPoints { get; set; }

        /// <summary>
        /// Name and Level of that skill.
        /// </summary>
        public Dictionary<string, int> Skills = new Dictionary<string, int>();

        public WafclastCharacter()
        {
            LifePoints = new WafclastStatePoints(CalculateLifePoints());
            ManaPoints = new WafclastStatePoints(CalculateManaPoints());
            EvasionPoints = CalculateEvasionPoints();
            DexteryPoints = CalculateDexteryPoints();
        }

        public double CalculateLifePoints() => (Constitution * 8.0) + (Strength / 5.0) + ((Constitution / 5.0) * 3.0);
        public double CalculateManaPoints() => (Intelligence * 3.0) + (Perception / 3.0) + (Constitution / 3.0);

        public abstract double CalculateDamagePoints();
        public double CalculatePhysicalDamage() => Strength + ((Strength / 5.0) * 2.0) + (Dexterity / 2.0);
        public double CalculateMagicalDamage() => Intelligence + ((Intelligence / 5.0) * 2.0) + (Dexterity / 2.0);

        public double CalculateEvasionPoints() => (Agility * 2.0) + (Dexterity / 5.0) + (Intelligence / 5.0) + (Perception / 3.0) + (Willpower / 3.0);
        public double CalculateDexteryPoints() => (Dexterity * 2.0) + (Agility / 5.0) + (Intelligence / 5.0) + (Perception / 3.0) + (Willpower / 3.0);

        public abstract void ResetCombatThings();

    }
}
