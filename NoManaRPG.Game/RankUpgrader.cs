// This file is part of WafclastRPG project.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Game
{
    public class RankUpgrader
    {

        [BsonId]
        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
        public int Rank { get; set; }
    }
}
