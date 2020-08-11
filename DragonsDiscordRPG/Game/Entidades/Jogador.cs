using MongoDB.Bson.Serialization.Attributes;

namespace DragonsDiscordRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class Jogador
    {
        [BsonId]
        public ulong Id { get; set; }
        public ulong IdVoz { get; set; }
    }
}

