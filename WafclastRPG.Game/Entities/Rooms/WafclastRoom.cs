// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities.Monsters;

namespace WafclastRPG.Game.Entities.Rooms
{
    [BsonIgnoreExtraElements]
    public class WafclastRoom : WafclastBaseRoom
    {
        public WafclastMonster Monster { get; set; }
    }
}
