// This file is part of WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Monsters
{
    [BsonIgnoreExtraElements]
    public class WafclastMonster
    {
        public string Name { get; set; }

        public double LifePoints { get; set; }
        public double Damage { get; set; }

        public List<WafclastMonsterItemDrop> ItemDrop { get; set; } = new List<WafclastMonsterItemDrop>();
    }
}
