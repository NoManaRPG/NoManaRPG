// This file is part of WafclastRPG project.

using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NoManaRPG.Game.Entities.Monsters;

namespace NoManaRPG.Game.Entities.Rooms
{
    [BsonIgnoreExtraElements]
    public class Room
    {
        [BsonId]
        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
        public ulong PlayerId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }

        public bool StarNotLostHalfLife { get; set; }
        public bool StarLessThanXTurn { get; set; }
        public bool StarZoneCompleted { get; set; }
        public List<Monster> Monsters { get; set; }

        public Room() { }

        public Room(Room zone)
        {
            this.Id = zone.Id;
            this.PlayerId = zone.PlayerId;
            this.Name = zone.Name;
            this.Level = zone.Level;
            this.StarNotLostHalfLife = zone.StarNotLostHalfLife;
            this.StarLessThanXTurn = zone.StarLessThanXTurn;
            this.StarZoneCompleted = zone.StarZoneCompleted;
            this.Monsters = zone.Monsters;
        }
    }
}
