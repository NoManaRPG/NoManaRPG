// This file is part of the WafclastRPG project.

using System;
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Characters;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastPlayer
    {
        [BsonId]
        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastBaseCharacter Character { get; private set; }

        public string Mention => $"<@{this.Id.ToString(CultureInfo.InvariantCulture)}>";
        public string Language { get; set; } = "pt-BR";

        public WafclastPlayer(ulong id, WafclastBaseCharacter Character)
        {
            this.Id = id;
            this.Character = Character;
            this.DateAccountCreation = DateTime.UtcNow;
        }
    }
}
