using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastJogador
    {
        [BsonId]
        public ulong Id { get; private set; }
        [BsonElement]
        public DateTime ContaCriacao { get; }
        public WafclastPersonagem Personagem { get; private set; }

        public WafclastJogador(ulong id)
        {
            this.Id = id;
            this.ContaCriacao = DateTime.UtcNow;
            this.Personagem = new WafclastPersonagem();
        }

        public WafclastJogador(WafclastJogador jogador)
        {
            this.Id = jogador.Id;
            this.ContaCriacao = jogador.ContaCriacao;
            this.Personagem = jogador.Personagem;
        }
    }
}
