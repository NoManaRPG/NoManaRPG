// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Rooms;
using WafclastRPG.Game.Entities.Skills;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Characters
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(WafclastCharacterMage),
                    typeof(WafclastCharacterWarrior))]
    public abstract class WafclastBaseCharacter : WafclastLevel
    {
        #region Attributes
        /// <summary>
        /// All 6 attributes.
        /// </summary>
        public abstract WafclastAttributes Attributes { get; set; }

        /// <summary>
        /// Points to Allocate in Attributes after level up
        /// </summary>
        public int AttributePoints { get; set; } = 5;
        #endregion
        #region Counter
        public ulong MonsterKills { get; set; }
        public ulong Deaths { get; set; }
        #endregion
        #region Status Points
        /// <summary>
        /// Pure damage
        /// </summary>
        public double Damage { get; set; }

        /// <summary>
        /// Points to evade an attack
        /// </summary>
        public double EvasionPoints { get; set; }
        /// <summary>
        /// Points to hit an attack
        /// </summary>
        public double PrecisionPoints { get; set; }
        /// <summary>
        /// Points to determine next attack
        /// </summary>
        public double AttackSpeed { get; set; }
        #endregion


        public WafclastStatePoints LifePoints { get; set; }

        public abstract string EmojiAttack { get; set; }


        public WafclastBaseRoom Room { get; set; }

        public WafclastBaseCharacter()
        {
            this.LifePoints = new WafclastStatePoints(CalculateLifePoints(this.Attributes));
            this.EvasionPoints = CalculateEvasionPoints(this.Attributes);
            this.PrecisionPoints = CalculateDexteryPoints(this.Attributes);
            this.Damage = this.CalculateDamagePoints();
            this.AttackSpeed = CalculateAttackSpeed(this.Attributes);
        }

        public abstract double CalculateDamagePoints();
        public abstract void ResetCombatThings();

        public double ReceiveDamage(double valor)
        {
            this.LifePoints.Remove(valor);
            return valor;
        }

        public bool IsDead => this.LifePoints.Current <= 0;

    }
}
