using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Entities.Characters
{
    public class WafclastMage : WafclastCharacter
    {
        public override double CalculateDamagePoints()
        {
            return CalculateMagicalDamage();
        }

        public override void ResetCombatThings()
        {

        }
    }
}
