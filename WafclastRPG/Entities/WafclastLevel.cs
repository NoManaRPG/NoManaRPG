using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace WafclastRPG.Entities
{
    public abstract class WafclastLevel
    {
        public int Level { get; set; } = 1;
        public decimal CurrentExperience { get; set; }
        public decimal ExperienceForNextLevel { get; set; }
        public int BlockedLevel { get; set; }

        public WafclastLevel() => this.ExperienceForNextLevel = this.ExperienceTotalLevel(2);

        public WafclastLevel(int startLevel)
        {
            this.Level = startLevel;
            this.ExperienceForNextLevel = this.ExperienceTotalLevel(startLevel + 1);
        }

        public int AddExperience(decimal experience)
        {
            int niveisEv = 0;
            decimal expResultante = this.CurrentExperience + experience;
            if (expResultante >= this.ExperienceForNextLevel)
            {
                do
                {
                    expResultante = expResultante - this.ExperienceForNextLevel;
                    this.Evolve();
                    niveisEv++;
                } while (expResultante >= this.ExperienceForNextLevel);
                this.CurrentExperience += expResultante;
                return niveisEv;
            }
            this.CurrentExperience += experience;
            return niveisEv;
        }

        public void RemoveOneLevel()
        {
            CurrentExperience = 0;

            if (Level > BlockedLevel + 1)
                BlockedLevel = Level;

            Level--;
            ExperienceForNextLevel = this.ExperienceTotalLevel(Level + 1);
            this.CurrentExperience = this.ExperienceTotalLevel(Level);
        }

        private void Evolve()
        {
            this.Level++;
            this.ExperienceForNextLevel = this.ExperienceTotalLevel(Level + 1);
        }

        // Needs to be 83 for level 2.
        private int ExperienceTotalLevel(int level)
        {
            double v1 = 1.0 / 8.0 * level * (level - 1.0) + 75.0;
            double pow1 = Math.Pow(3, (level - 1.0) / 7.0) - 1;
            double pow2 = 1 - Math.Pow(2, -1 / 7.0);
            return (int)Math.Truncate(v1 * (pow1 / pow2));
        }

        public static void MapBuilderLevel()
        {
            BsonClassMap.RegisterClassMap<WafclastLevel>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(c => c.CurrentExperience).SetSerializer(new DecimalSerializer(BsonType.Decimal128, new RepresentationConverter(true, true)));
                cm.MapMember(c => c.ExperienceForNextLevel).SetSerializer(new DecimalSerializer(BsonType.Decimal128, new RepresentationConverter(true, true)));
            });
        }
    }
}
