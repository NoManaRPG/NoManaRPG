// This file is part of WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game
{
    public class RankUpgrader
    {

        [BsonId]
        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
        public int Rank { get; set; }
    }
}
