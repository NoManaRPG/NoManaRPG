using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TorreRPG.Services
{
    public class BancoSession
    {
        public IMongoDatabase Database { get; private set; }
        public IMongoCollection<RPJogador> Jogadores { get; private set; }

        private IClientSessionHandle session;

        public BancoSession(IClientSessionHandle session)
        {
            this.session = session;
            session.StartTransaction();
            Database = session.Client.GetDatabase("Dragon");
            Jogadores = Database.GetCollection<RPJogador>(nameof(RPJogador));
        }

        public Task<RPJogador> GetJogadorAsync(CommandContext ctx)
            => Jogadores.Find(session, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();

        public Task<RPJogador> GetJogadorAsync(DiscordUser user)
            => Jogadores.Find(session, x => x.Id == user.Id).FirstOrDefaultAsync();

        public Task EditJogadorAsync(RPJogador jogador)
            => Jogadores.ReplaceOneAsync(session, x => x.Id == jogador.Id, jogador);

        public Task AddJogadorAsync(RPJogador jogador)
            => Jogadores.InsertOneAsync(session, jogador);
    }
}
