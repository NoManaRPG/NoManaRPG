// This file is part of the WafclastRPG project.

using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastLevel
    {
        public int Level { get; set; } = 1;
        public double CurrentExperience { get; set; }
        public double ExperienceForNextLevel { get; set; }
        public int BlockedLevel { get; set; }

        public WafclastLevel()
        {
            this.ExperienceForNextLevel = this.ExperienceTotalLevel(2);
        }

        public WafclastLevel(int startLevel)
        {
            this.Level = startLevel;
            this.ExperienceForNextLevel = this.ExperienceTotalLevel(startLevel + 1);
        }

        public int AddExperience(double experience)
        {
            int niveisEv = 0;
            this.CurrentExperience += experience;
            if (this.CurrentExperience >= this.ExperienceForNextLevel)
            {
                do
                {
                    this.Evolve();
                    niveisEv++;
                } while (this.CurrentExperience >= this.ExperienceForNextLevel);
            }
            return niveisEv;
        }

        public void RemoveOneLevel()
        {
            this.CurrentExperience = 0;

            if (this.Level == 1)
                return;

            if (this.Level > this.BlockedLevel + 1)
                this.BlockedLevel = this.Level;

            this.Level--;
            this.ExperienceForNextLevel = this.ExperienceTotalLevel(this.Level + 1);
            this.CurrentExperience = this.ExperienceTotalLevel(this.Level);
        }

        private void Evolve()
        {
            this.Level++;
            this.ExperienceForNextLevel = this.ExperienceTotalLevel(this.Level + 1);
        }

        public int ExperienceTotalLevel(int level)
        {
            double v1 = 1.0 / 8.0 * level * (level - 1.0) + 75.0;
            double pow1 = Math.Pow(3, (level - 1.0) / 7.0) - 1;
            double pow2 = 1 - Math.Pow(2, -1 / 7.0);
            return (int)Math.Truncate(v1 * (pow1 / pow2));
        }
    }
}
