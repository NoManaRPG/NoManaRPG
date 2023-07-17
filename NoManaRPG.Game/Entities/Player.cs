// This file is part of NoManaRPG project.

using System;
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Game.Entities;

[BsonIgnoreExtraElements]
public class Player : Level
{
    [BsonId]
    public ulong DiscordId { get; private set; }
    public DateTime DateAccountCreation { get; private set; }
    public ulong MonstersKills { get; private set; }
    public ulong PlayersKills { get; private set; }
    public ulong Deaths { get; private set; }

    public Character Character { get; private set; }


    public string Mention => $"<@{this.DiscordId.ToString(CultureInfo.InvariantCulture)}>";
    public string Language { get; private set; } = "";

    public Player(ulong id)
    {
        this.DiscordId = id;
        this.DateAccountCreation = DateTime.UtcNow;
        this.MonstersKills = 0;
        this.PlayersKills = 0;
        this.Deaths = 0;

        this.Character = new Character();
    }
}
