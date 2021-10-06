// This file is part of the WafclastRPG project.

namespace WafclastRPG.Game.Entities.Skills.Effects
{
    public class WafclastSkillStunEffect : WafclastSkillBaseEffect
    {
        public double RoundStun { get; set; }

        public WafclastSkillStunEffect(double stunTime)
        {
            this.RoundStun = stunTime;
        }
    }
}
