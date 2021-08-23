using System;

namespace WafclastRPG.Entities.Characters
{
    public class WafclastWarrior : WafclastCharacter
    {
        public override double CalculateDamagePoints()
        {
            return CalculatePhysicalDamage();
        }

        public override void ResetCombatThings()
        {

        }
    }
}
