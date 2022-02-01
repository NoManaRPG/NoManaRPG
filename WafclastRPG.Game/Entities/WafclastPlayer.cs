// This file is part of WafclastRPG project.

using System;
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastPlayer : WafclastLevel
    {
        [BsonId]
        public ulong DiscordId { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public ulong MonstersKills { get; private set; }
        public ulong PlayersKills { get; private set; }
        public ulong Deaths { get; private set; }

        public WafclastCharacter Character { get; private set; }


        public string Mention => $"<@{this.DiscordId.ToString(CultureInfo.InvariantCulture)}>";
        public string Language { get; private set; } = "";

        public WafclastPlayer(ulong id)
        {
            this.DiscordId = id;
            this.DateAccountCreation = DateTime.UtcNow;
            this.MonstersKills = 0;
            this.PlayersKills = 0;
            this.Deaths = 0;

            this.Character = new WafclastCharacter();
        }
    }
}
