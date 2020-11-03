using DSharpPlus.CommandsNext;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPJogador
    {
        [BsonId]
        public ulong Id { get; private set; }
        public RPPersonagem Personagem { get; private set; }
        public DateTime Criacao { get; private set; }

        public RPJogador(CommandContext ctx, RPPersonagem personagem)
        {
            Id = ctx.User.Id;
            Personagem = personagem;
            Criacao = DateTime.UtcNow;
        }
    }
}
