// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Game.Characters
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacterWarrior : WafclastBaseCharacter
    {
        public override string EmojiAttack { get; set; } = Emojis.Adaga;

        public WafclastStatePoints SR { get; set; } = new WafclastStatePoints(10);

        public override (string, string) Energia => ("Força de vontade", $"{Emojis.DinamicHeartEmoji(this.SR)} {this.SR.Current:N2}");

        public WafclastCharacterWarrior()
        {

        }

        public override double CalculateDamagePoints() => Mathematics.CalculatePhysicalDamage(this.Attributes);
    }
}
