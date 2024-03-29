// This file is part of NoManaRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Entidades.Monsters;

[BsonIgnoreExtraElements]
public class Monster
{
    public string Name { get; set; }

    public double LifePoints { get; set; }
    public double Damage { get; set; }

    public List<MonsterItemDrop> ItemDrop { get; set; } = new List<MonsterItemDrop>();
}
