// This file is part of WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Game.Entities.Monsters
{
    [BsonIgnoreExtraElements]
    public class Monster
    {
        public string Name { get; set; }

        public double LifePoints { get; set; }
        public double Damage { get; set; }

        public List<MonsterItemDrop> ItemDrop { get; set; } = new List<MonsterItemDrop>();
    }
}
