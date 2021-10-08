// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Characters
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacterMage : WafclastBaseCharacter
    {
        public WafclastCharacterMage(DamageType damageType = DamageType.Magic) : base(damageType)
        {
        }

        public override string EmojiAttack { get; set; } = Emojis.Dardo;

        public override string ResourcePointsName => "Mana";
    }
}
