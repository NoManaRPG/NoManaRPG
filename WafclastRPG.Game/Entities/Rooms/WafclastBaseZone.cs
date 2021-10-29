// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Rooms
{
    [BsonIgnoreExtraElements]
    public class WafclastBaseZone
    {
        [BsonId]
        public ulong Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public int Level { get; set; }

        public bool StarNotLostHalfLife { get; set; }
        public bool StarLessThanXTurn { get; set; }
        public bool StarZoneCompleted { get; set; }

        public WafclastBaseZone() { }

        public WafclastBaseZone(WafclastBaseZone zone)
        {
            this.Id = zone.Id;
            this.Name = zone.Name;
            this.Rank = zone.Rank;
            this.Level = zone.Level;
            this.StarNotLostHalfLife = zone.StarNotLostHalfLife;
            this.StarLessThanXTurn = zone.StarLessThanXTurn;
            this.StarZoneCompleted = zone.StarZoneCompleted;
        }
    }
}
