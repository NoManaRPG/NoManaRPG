using TorreRPG.Entidades;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace TorreRPG.Services
{
    public class BancoSession
    {
        public IMongoDatabase Database { get; }
        public IMongoCollection<RPJogador> Jogadores { get; }

        private IClientSessionHandle Session { get; }

        public BancoSession(IClientSessionHandle session)
        {
            this.Session = session;
            session.StartTransaction();
            Database = session.Client.GetDatabase("Dragon");
            Jogadores = Database.GetCollection<RPJogador>(nameof(RPJogador));
        }

        public Task<RPJogador> GetJogadorAsync(CommandContext ctx)
            => Jogadores.Find(Session, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();

        public Task<RPJogador> GetJogadorAsync(DiscordUser user)
            => Jogadores.Find(Session, x => x.Id == user.Id).FirstOrDefaultAsync();

        public Task EditJogadorAsync(RPJogador jogador)
            => Jogadores.ReplaceOneAsync(Session, x => x.Id == jogador.Id, jogador);

        public Task AddJogadorAsync(RPJogador jogador)
            => Jogadores.InsertOneAsync(Session, jogador);
    }
}
