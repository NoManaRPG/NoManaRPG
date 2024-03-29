// This file is part of NoManaRPG project.

using System;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Entidades;

[BsonIgnoreExtraElements]
public class Level
{
    public int ActualLevel { get; private set; } = 1;
    public double CurrentExperience { get; private set; }
    public double ExperienceForNextLevel { get; private set; }

    public Level()
    {
        this.ExperienceForNextLevel = this.ExperienceTotalLevel(2);
    }

    public Level(int startLevel)
    {
        this.ActualLevel = startLevel;
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

    private void Evolve()
    {
        this.ActualLevel++;
        this.ExperienceForNextLevel = this.ExperienceTotalLevel(this.ActualLevel + 1);
    }

    private int ExperienceTotalLevel(int level)
    {
        double v1 = 1.0 / 8.0 * level * (level - 1.0) + 75.0;
        double pow1 = Math.Pow(3, (level - 1.0) / 7.0) - 1;
        double pow2 = 1 - Math.Pow(2, -1 / 7.0);
        return (int)Math.Truncate(v1 * (pow1 / pow2));
    }
}
