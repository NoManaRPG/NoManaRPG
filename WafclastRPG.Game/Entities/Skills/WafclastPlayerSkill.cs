// This file is part of the WafclastRPG project.

namespace WafclastRPG.Game.Entities.Skills
{
    public class WafclastPlayerSkill : WafclastBaseSkill
    {
        public ulong PlayerId { get; set; }

        public WafclastPlayerSkill(string name, ulong playerId) : base(name)
        {
        }
    }
}
