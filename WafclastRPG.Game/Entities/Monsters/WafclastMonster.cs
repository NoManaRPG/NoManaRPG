// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Enums;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Entities.Monsters
{
    [BsonIgnoreExtraElements]
    public class WafclastMonster
    {
        public int Level { get; set; }
        public string Name { get; set; }

        public string Mention => $"{this.Name} [Nv. {this.Level}]";

        public WafclastAttributes Attributes { get; set; }
        public WafclastStatePoints LifePoints { get; set; }


        public DamageType DamageType { get; set; }
        public double Damage { get; set; }
        public double EvasionPoints { get; set; }
        public double PrecisionPoints { get; set; }
        public double AttackSpeed { get; set; }

        public List<DropChance> Drops { get; set; } = new List<DropChance>();

        public WafclastMonster(int level, string name, DamageType damageType, double strength = 5, double constitution = 5, double dexterity = 5, double agility = 5, double intelligence = 5, double willpower = 5, double perception = 5, double charisma = 5)
        {
            this.Level = level;
            this.Name = name;
            this.DamageType = damageType;
            this.Attributes = new WafclastAttributes(strength, constitution, dexterity, intelligence, willpower, perception, charisma);
        }

        public void CalculateStatistics()
        {
            this.LifePoints = new WafclastStatePoints(CalculateLifePoints(this.Attributes));
            this.EvasionPoints = CalculateEvasionPoints(this.Attributes);
            this.PrecisionPoints = CalculateDexteryPoints(this.Attributes);
            this.AttackSpeed = CalculateAttackSpeed(this.Attributes);
            if (this.DamageType == DamageType.Magic) this.Damage = CalculateMagicalDamage(this.Attributes);
            else
                this.Damage = CalculatePhysicalDamage(this.Attributes);
        }

        public double TakeDamage(double valor)
        {
            this.LifePoints.Remove(valor);
            return valor;
        }
    }

    public class DropChance
    {
        public int GlobalItemId { get; set; }
        public double Chance { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }

        public DropChance() { }

        public DropChance(int globalItemId, double chance, int minQuantity, int maxQuantity)
        {
            this.GlobalItemId = globalItemId;
            this.Chance = chance;
            this.MinQuantity = minQuantity;
            this.MaxQuantity = maxQuantity;
        }
    }
}
