using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.MercadoGeral;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.DataBases
{
    public class DatabaseSession : IDisposable
    {
        public IClientSessionHandle Session { get; }
        public DataBase Database { get; }

        public DatabaseSession(IClientSessionHandle session, DataBase database)
        {
            Session = session;
            Database = database;
        }

        public Task<Response> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<Response>> callbackAsync) => Session.WithTransactionAsync(callbackAsync: callbackAsync);


        public async Task<WafclastPlayer> FindPlayerAsync(DiscordUser user)
        {
            var player = await Database.CollectionPlayers.Find(Session, x => x.Id == user.Id).FirstOrDefaultAsync();
            if (player != null)
                player.Session = this;
            return player;
        }

        /// <summary>
        /// Procura um item pelo o seu ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>WafclastBaseItem</returns>
        public Task<WafclastBaseItem> FindItemAsync(ObjectId id)
         => Database.CollectionItems.Find(Session, x => x.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Procura uma ordem pelo o seu ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ordem</returns>
        public Task<Ordem> FindOrdemAsync(ObjectId id)
            => Database.CollectionOrdens.Find(Session, x => x.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Procura uma ação pelo o seu Nome.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns>Acao</returns>
        public Task<Acao> FindAcaoAsync(string nome)
            => Database.CollectionAcoes.Find(x => x.Id == nome, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();

        public Task ReplaceAsync(WafclastPlayer jogador)
         => Database.CollectionPlayers.ReplaceOneAsync(Session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastServer server)
          => Database.CollectionGuilds.ReplaceOneAsync(Session, x => x.Id == server.Id, server, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastBaseItem item)
          => Database.CollectionItems.ReplaceOneAsync(Session, x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastMonster monster)
           => Database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster, new ReplaceOptions { IsUpsert = true });


        public Task InsertAsync(WafclastBaseItem item)
            => Database.CollectionItems.InsertOneAsync(Session, item);
        public Task InsertAsync(Acao acao)
            => Database.CollectionAcoes.InsertOneAsync(Session, acao);
        public Task InsertAsync(Ordem ordem)
            => Database.CollectionOrdens.InsertOneAsync(Session, ordem);

        public Task RemoveAsync(WafclastBaseItem item)
           => Database.CollectionItems.DeleteOneAsync(Session, x => x.Id == item.Id);

        public void Dispose() => Session.Dispose();
    }

    public class Response
    {
        public DiscordEmbedBuilder Embed;
        public ulong? TargetId;
        public string Message;
        public Response(DiscordEmbedBuilder embed) => Embed = embed;
        public Response(string message) => Message = message;
    }
}
