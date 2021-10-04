// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Entities.Monsters;

namespace WafclastRPG.Game.Entities.Rooms
{
    [BsonIgnoreExtraElements]
    public class WafclastRoom
    {
        [BsonId]
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public WafclastVector Location { get; set; }
        public string Invite { get; set; }

        public WafclastMonster Monster { get; set; }
        public WafclastRoomAttackOrder AttackOrder { get; set; }

        public string Mention => $"<#{this.Id}>";
    }
}
