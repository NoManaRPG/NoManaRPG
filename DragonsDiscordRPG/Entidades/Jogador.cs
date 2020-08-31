using MongoDB.Bson.Serialization.Attributes;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class Jogador
    {
        [BsonId]
        public ulong Id { get; private set; }
        public ulong IdVoz { get; private set; }
    }
}

