// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Rooms;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Characters
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(WafclastCharacterMage), typeof(WafclastCharacterWarrior))]
    public abstract class WafclastBaseCharacter : WafclastLevel
    {

        /// <summary>
        /// All 6 attributes.
        /// </summary>
        public abstract WafclastAttributes Attributes { get; set; }

        /// <summary>
        /// Points to Allocate in Attributes after level up
        /// </summary>
        public int AttributePoints { get; set; } = 5;

        public WafclastStatePoints LifePoints { get; set; }
        public WafclastStatePoints ManaPoints { get; set; }

        public double Damage { get; set; }

        public ulong MonsterKills { get; set; }
        public ulong Deaths { get; set; }


        public double EvasionPoints { get; set; }
        public double PrecisionPoints { get; set; }

        public double AttackSpeed { get; set; }

        public abstract string EmojiAttack { get; set; }

        /// <summary>
        /// Name and Level of that skill.
        /// </summary>
        public Dictionary<string, int> Skills = new Dictionary<string, int>();

        public WafclastRoom Room { get; set; }

        public WafclastBaseCharacter()
        {
            this.LifePoints = new WafclastStatePoints(CalculateLifePoints(this.Attributes));
            this.ManaPoints = new WafclastStatePoints(CalculateManaPoints(this.Attributes));
            this.EvasionPoints = CalculateEvasionPoints(this.Attributes);
            this.PrecisionPoints = CalculateDexteryPoints(this.Attributes);
            this.Damage = this.CalculateDamagePoints();
            this.AttackSpeed = CalculateAttackSpeed(this.Attributes);
        }

        public abstract double CalculateDamagePoints();
        public abstract void ResetCombatThings();

        public (bool isPlayer, bool isMonster) NextAttack()
          => this.Room.AttackOrder.CalculateNextAttack(this.AttackSpeed, this.Room.Monster.AttackSpeed);

        public double ReceiveDamage(double valor)
        {
            this.LifePoints.Remove(valor);
            return valor;
        }

        public bool IsDead => this.LifePoints.Current <= 0;

    }
}
