// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities.Monsters;

namespace WafclastRPG.Game.Entities.Rooms
{
    [BsonIgnoreExtraElements]
    public class WafclastZone : WafclastBaseZone
    {
        public List<WafclastMonster> Monsters { get; set; }
    }
}
