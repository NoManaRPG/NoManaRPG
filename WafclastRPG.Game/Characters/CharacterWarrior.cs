// This file is part of the WafclastRPG project.

using WafclastRPG.Game.Entities;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Characters
{
    public class CharacterWarrior : BaseCharacter
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
