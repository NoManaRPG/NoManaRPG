// This file is part of WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game
{
    public class RankUpgrader
    {

        [BsonId]
        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
        public TypeUpgrader TypeUp { get; set; }
        public int Rank { get; set; }
        public ICollection<WafclastItem> Itens { get; set; }
    }
}
