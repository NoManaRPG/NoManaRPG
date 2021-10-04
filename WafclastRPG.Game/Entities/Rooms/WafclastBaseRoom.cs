// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Rooms
{
    [BsonIgnoreExtraElements]
    public class WafclastBaseRoom
    {
        [BsonId]
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Description { get; set; }
        public WafclastVector Location { get; set; }
        public string Invite { get; set; }

        public string Mention => $"<#{this.Id}>";

        public WafclastBaseRoom() { }

        public WafclastBaseRoom(WafclastBaseRoom room)
        {
            this.Id = room.Id;
            this.Name = room.Name;
            this.Region = room.Region;
            this.Description = room.Description;
            this.Location = room.Location;
            this.Invite = room.Invite;
        }
    }
}
