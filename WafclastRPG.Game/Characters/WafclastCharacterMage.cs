// This file is part of the WafclastRPG project.

using WafclastRPG.Game.Entities;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Characters
{
    public class WafclastCharacterMage : WafclastBaseCharacter
    {

        public override string EmojiAttack { get; set; } = Emojis.Dardo;
        public override WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

        public WafclastCharacterMage()
        {
        }
        public override double CalculateDamagePoints()
        {
            return CalculateMagicalDamage(this.Attributes);
        }

        public override void ResetCombatThings()
        {

        }
    }
}
