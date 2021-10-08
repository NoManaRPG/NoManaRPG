// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Characters
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacterWarrior : WafclastBaseCharacter
    {
        public override string EmojiAttack { get; set; } = Emojis.Adaga;

        public override string ResourcePointsName => "Vontade";

        public WafclastCharacterWarrior(DamageType damageType = DamageType.Physic) : base(damageType)
        {
        }
    }
}
