// This file is part of the WafclastRPG project.

namespace WafclastRPG.Game.Entities.Skills.Effects
{
    public class WafclastSkillDamageEffect : WafclastSkillBaseEffect
    {
        public double Damage { get; set; }

        public WafclastSkillDamageEffect(double damage)
        {
            this.Damage = damage;
        }
    }
}
