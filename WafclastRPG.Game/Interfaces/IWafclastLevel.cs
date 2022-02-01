// This file is part of WafclastRPG project.

namespace WafclastRPG.Game.Interfaces
{
    public interface IWafclastLevel
    {
        int Level { get; }
        double CurrentExperience { get; }
        double ExperienceForNextLevel { get; }

        int AddExperience(double experience);
    }
}
