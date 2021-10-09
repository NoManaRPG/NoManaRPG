// This file is part of the WafclastRPG project.

using System.Collections.ObjectModel;
using MongoDB.Bson.Serialization.Attributes;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Entities.Monsters
{
    [BsonIgnoreExtraElements]
    public class WafclastMonster
    {
        public int Level { get; set; }

        public string Name { get; set; }
        public string NameLevel => $"{this.Name} [Nv. {this.Level}]";

        private WafclastAttributes Attributes { get; set; }

        public double EvasionPoints { get; set; }
        public double PrecisionPoints { get; set; }
        public double AttackSpeed { get; set; }
        public double Damage { get; set; }
        public double LifePoints { get; set; }

        public Collection<WafclastMonsterReward> Rewards { get; set; }

        public WafclastMonster(int level, string name, double strength = 5, double constitution = 5, double dexterity = 5, double agility = 5, double intelligence = 5, double willpower = 5, double perception = 5, double charisma = 5)
        {
            this.Level = level;
            this.Name = name;
            this.Attributes = new WafclastAttributes(strength, constitution, dexterity, intelligence, willpower, perception, charisma);
            this.LifePoints = CalculateLifePoints(this.Attributes);
            this.EvasionPoints = CalculateEvasionPoints(this.Attributes);
            this.PrecisionPoints = CalculatePrecisionPoints(this.Attributes);
            this.AttackSpeed = CalculateAttackSpeed(this.Attributes);
            this.Damage = CalculatePhysicalDamage(this.Attributes);
            this.Rewards = new();
        }
    }
}
