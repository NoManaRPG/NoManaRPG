// This file is part of the WafclastRPG project.

using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Wafclast
{
    [BsonIgnoreExtraElements]
    public class Room
    {
        [BsonId]
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public Vector Location { get; set; }
        public string Invite { get; set; }

        public Monster Monster { get; set; }
        public RoomAttackOrder AttackOrder { get; set; }

        public string Mention { get => $"<#{this.Id}>"; }
    }
}
