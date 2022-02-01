// This file is part of WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Interfaces;

namespace WafclastRPG.Game.Entities.Jobs
{
    [BsonIgnoreExtraElements]
    public class WafclastJob : WafclastBaseJob, IWafclastLevel
    {
        public ulong DiscordId { get; private set; }

        public WafclastJob()
        {
            _level = new WafclastLevel();
        }

        private WafclastLevel _level;
        public int Level => _level.Level;
        public double CurrentExperience => _level.CurrentExperience;
        public double ExperienceForNextLevel => _level.ExperienceForNextLevel;
        public int AddExperience(double experience) => _level.AddExperience(experience);
    }
}
