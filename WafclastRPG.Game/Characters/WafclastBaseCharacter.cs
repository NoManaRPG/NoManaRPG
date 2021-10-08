// This file is part of the WafclastRPG project.

using System;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Rooms;
using WafclastRPG.Game.Enums;
using WafclastRPG.Game.Properties;
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
        public WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

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

        #region Damage
        /// <summary>
        /// Pure damage
        /// </summary>
        public double Damage { get; set; }
        public DamageType DamageType { get; set; }
        #endregion
        #region LifePoints
        public WafclastStatePoints LifePoints { get; set; }
        public virtual string LifePointsName => Messages.PontosDeVida;
        public string LifePointsStatus => $"{Emojis.DinamicHeartEmoji(this.LifePoints)} {this.LifePoints.Current:N2}";
        #endregion
        #region Resource
        public WafclastStatePoints ResourcePoints { get; set; }
        public abstract string ResourcePointsName { get; }
        public string ResourcePointsStatus => $"{Emojis.DinamicHeartEmoji(this.ResourcePoints)} {this.ResourcePoints.Current:N2}";
        #endregion

        public abstract string EmojiAttack { get; set; }

        public WafclastBaseRoom Room { get; set; }

        public WafclastBaseCharacter(DamageType damageType)
        {
            this.DamageType = damageType;
            this.LifePoints = new WafclastStatePoints(CalculateLifePoints(this.Attributes));
            this.EvasionPoints = CalculateEvasionPoints(this.Attributes);
            this.PrecisionPoints = CalculatePrecisionPoints(this.Attributes);
            this.AttackSpeed = CalculateAttackSpeed(this.Attributes);
            this.Damage = this.CalculateDamagePoints();
            this.ResourcePoints = new WafclastStatePoints(this.CalculateResourcePoints());
        }

        public double CalculateDamagePoints() => this.DamageType switch
        {
            DamageType.Physic => CalculatePhysicalDamage(this.Attributes),
            DamageType.Magic => CalculateMagicalDamage(this.Attributes),
            _ => throw new Exception("Dano não tratado!"),
        };

        public double CalculateResourcePoints() => this.DamageType switch
        {
            DamageType.Physic => CalculateResourceWarrior(this.Attributes),
            DamageType.Magic => CalculateManaPoints(this.Attributes),
            _ => throw new Exception("Recurso não tratado!"),
        };
    }
}
