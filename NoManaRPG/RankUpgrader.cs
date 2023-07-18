// This file is part of NoManaRPG project.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG;

public class RankUpgrader
{

    [BsonId]
    public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
    public int Rank { get; set; }
}
