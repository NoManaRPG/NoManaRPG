// This file is part of the WafclastRPG project.

using System;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Wafclast
{
    [BsonIgnoreExtraElements]
    public class Room : IEquatable<Room>
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
        #region Operator
        public bool Equals(Room other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return other.Id == this.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(Room))
                return false;
            return this.Equals((Room)obj);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(Room left, Room right) => Equals(left, right);
        public static bool operator !=(Room left, Room right) => !Equals(left, right);
        #endregion

    }
}
