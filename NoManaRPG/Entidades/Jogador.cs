// This file is part of NoManaRPG project.

using System;
using MongoDB.Bson.Serialization.Attributes;

namespace NoManaRPG.Entidades;

[BsonIgnoreExtraElements]
public class Jogador
{
    [BsonId]
    public ulong DiscordId { get; private set; }
    public DateTime DateAccountCreation { get; private set; }
    public ulong MonstersKills { get; private set; }
    public ulong PlayersKills { get; private set; }
    public ulong Deaths { get; private set; }

    public Personagem Personagem { get; private set; }

    public string Language { get; private set; } = "";

    public Jogador(ulong id)
    {
        this.DiscordId = id;
        this.DateAccountCreation = DateTime.UtcNow;
        this.MonstersKills = 0;
        this.PlayersKills = 0;
        this.Deaths = 0;

        this.Personagem = new();
    }
}
