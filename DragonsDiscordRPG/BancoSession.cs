using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG
{
    public class BancoSession
    {
        public IMongoDatabase Database { get; private set; }
        public IMongoCollection<RPJogador> ColecaoJogador { get; private set; }

        private IClientSessionHandle session;

        public BancoSession(IClientSessionHandle session)
        {
            this.session = session;
            session.StartTransaction();
            Database = session.Client.GetDatabase("Dragon");
            ColecaoJogador = Database.GetCollection<RPJogador>(nameof(RPJogador));
        }

        public Task<RPJogador> GetJogadorAsync(CommandContext ctx)
            => ColecaoJogador.Find(session, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();

        public Task EditJogadorAsync(RPJogador jogador)
            => ColecaoJogador.ReplaceOneAsync(session, x => x.Id == jogador.Id, jogador);

        public Task AddJogadorAsync(RPJogador jogador)
            => ColecaoJogador.InsertOneAsync(session, jogador);
    }
}
