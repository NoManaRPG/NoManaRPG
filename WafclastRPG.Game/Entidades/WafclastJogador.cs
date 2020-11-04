using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastJogador
    {
        [BsonId]
        public ulong Id { get; }
        [BsonElement]
        public DateTime ContaCriacao { get; }
        public WafclastPersonagem Personagem { get; private set; }

        public WafclastJogador(ulong id, WafclastPersonagem personagem)
        {
            this.Id = id;
            this.ContaCriacao = DateTime.UtcNow;
            this.Personagem = personagem;
        }
    }
}
