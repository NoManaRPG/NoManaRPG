// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Game.Characters
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacterMage : WafclastBaseCharacter
    {
        public override string EmojiAttack { get; set; } = Emojis.Dardo;

        public override (string, string) Energia => throw new System.NotImplementedException();

        public WafclastCharacterMage()
        {
        }

        public override double CalculateDamagePoints() => Mathematics.CalculateMagicalDamage(this.Attributes);
    }
}
