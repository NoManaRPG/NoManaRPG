using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastJogador
    {
        [BsonId]
        public ulong Id { get; }
        public DateTime ContaCriacao { get; }
        public WafclastPersonagem Personagem { get; private set; }

        public WafclastJogador(ulong id, WafclastPersonagem personagem)
        {
            this.Id = id;
            this.Personagem = personagem;
            this.ContaCriacao = DateTime.UtcNow;
        }
    }
}
