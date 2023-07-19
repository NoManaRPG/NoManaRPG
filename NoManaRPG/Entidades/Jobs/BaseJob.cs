// This file is part of NoManaRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Entidades.Jobs;

[BsonIgnoreExtraElements]
public class BaseJob
{
    public string Name { get; private set; }

    public double LifePointsBonus { get; private set; }
    public double ManaPointsBonus { get; private set; }

    public double PhysicalDamageBonus { get; private set; }
    public double MagicalDamageBonus { get; private set; }

    public double PhysicalDefenseBonus { get; private set; }
    public double MagicalDefenseBonus { get; private set; }

    public double LuckBonus { get; private set; }
    public double SpeedBonus { get; private set; }
}
