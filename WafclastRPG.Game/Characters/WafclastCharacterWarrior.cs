// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Characters
{
    [BsonIgnoreExtraElements]
    public class WafclastCharacterWarrior : WafclastBaseCharacter
    {
        public override string EmojiAttack { get; set; } = Emojis.Adaga;
        public override WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

        public override double CalculateDamagePoints()
        {
            return CalculatePhysicalDamage(this.Attributes);
        }

        public override void ResetCombatThings()
        {

        }
    }
}
