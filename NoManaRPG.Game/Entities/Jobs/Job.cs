// This file is part of WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Game.Entities.Jobs
{
    [BsonIgnoreExtraElements]
    public class Job : BaseJob
    {
        public ulong DiscordId { get; private set; }

        public Job()
        {
            _level = new Level();
        }

        private Level _level;
        public int Level => _level.ActualLevel;
        public double CurrentExperience => _level.CurrentExperience;
        public double ExperienceForNextLevel => _level.ExperienceForNextLevel;
        public int AddExperience(double experience) => _level.AddExperience(experience);
    }
}
