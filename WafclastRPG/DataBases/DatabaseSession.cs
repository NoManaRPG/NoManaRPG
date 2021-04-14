using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.DataBases
{
    public class DatabaseSession : IDisposable
    {
        public IClientSessionHandle Session { get; }
        private DataBase Database { get; }

        public DatabaseSession(IClientSessionHandle session, DataBase database)
        {
            Session = session;
            Database = database;
        }

        public Task<Response> WithTransactionAsync(Func<IClientSessionHandle, CancellationToken, Task<Response>> callbackAsync) => Session.WithTransactionAsync(callbackAsync: callbackAsync);

        public void Dispose() => Session.Dispose();

        #region FindAsync


        public Task<WafclastPlayer> FindAsync(DiscordUser user)
          => Database.CollectionPlayers.Find(Session, x => x.Id == user.Id).FirstOrDefaultAsync();
        public Task<WafclastMonster> FindMonsterAsync(int floorLevel)
          => Database.CollectionMonsters.AsQueryable().Sample(1).Where(x => x.FloorLevel <= floorLevel).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindItemAsync(ObjectId id)
         => Database.CollectionItems.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindAsync(string itemName, WafclastPlayer player)
                => Database.CollectionItems.Find(Session, x => x.PlayerId == player.Id && x.Name == itemName,
                    new FindOptions { Collation = new Collation("pt-BR", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();


        #endregion
        #region Replace


        public Task ReplaceAsync(WafclastPlayer jogador)
         => Database.CollectionPlayers.ReplaceOneAsync(Session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastServer server)
          => Database.CollectionGuilds.ReplaceOneAsync(Session, x => x.Id == server.Id, server, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastBaseItem item)
          => Database.CollectionItems.ReplaceOneAsync(Session, x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastMonster monster)
           => Database.CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster, new ReplaceOptions { IsUpsert = true });


        #endregion
        #region Insert


        public async Task InsertAsync(WafclastBaseItem item)
        {
            item.Id = ObjectId.Empty;
            await Database.CollectionItems.InsertOneAsync(Session, item);
        }
        public async Task InsertAsync(WafclastBaseItem item, int quantity, WafclastPlayer player)
        {
            item.PlayerId = player.Id;
            item.Quantity = 1;
            if (!item.CanStack)
            {
                for (int i = 0; i < quantity; i++)
                    await InsertAsync(item);
                return;
            }

            var itemFound = await Database.CollectionItems.Find(Session, x => x.Name == item.Name).FirstOrDefaultAsync();
            if (itemFound == null)
            {
                item.Quantity = quantity;
                await InsertAsync(item);
                return;
            }

            itemFound.Quantity += quantity;
            await ReplaceAsync(itemFound);
        }


        #endregion
        #region Remove


        public Task RemoveAsync(WafclastBaseItem item)
           => Database.CollectionItems.DeleteOneAsync(Session, x => x.Id == item.Id);


        #endregion
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
