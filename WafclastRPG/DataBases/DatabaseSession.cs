using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Maps;
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
        public Task<WafclastMonster> FindAsync(WafclastMonsterBase monster)
          => Database.CollectionMonsters.Find(Session, x => x.Id == monster.Id).FirstOrDefaultAsync();
        public Task<WafclastMap> FindAsync(DiscordChannel discordChannel)
            => Database.CollectionMaps.Find(Session, x => x.Id == discordChannel.Id).FirstOrDefaultAsync();

        /// <summary>
        /// Procura o ID + 1 do último item feito na Guilda.
        /// </summary>
        /// <param name="guild"></param>
        /// <returns>Retorna 1 caso não encontre nenhum item feito na Guilda.</returns>
        public async Task<ulong> FindAsync(DiscordGuild guild)
        {
            var item = await Database.CollectionItems.Find(Session, x => x.PlayerId == guild.Id).SortByDescending(x => x.ItemID).Limit(1).FirstOrDefaultAsync();
            if (item == null)
                return 1;
            return item.ItemID + 1;
        }
        public Task<WafclastBaseItem> FindAsync(ObjectId id)
         => Database.CollectionItems.Find(Session, x => x.Id == id).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindAsync(ulong itemId, DiscordGuild guild)
        => Database.CollectionItems.Find(Session, x => x.PlayerId == guild.Id && x.ItemID == guild.Id).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindAsync(ulong itemId, WafclastPlayer player)
        => Database.CollectionItems.Find(Session, x => x.PlayerId == player.Id && x.ItemID == itemId).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindAsync(string itemName, WafclastPlayer player)
                => Database.CollectionItems.Find(Session, x => x.PlayerId == player.Id && x.Name == itemName,
                    new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();


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
                item.Quantity = 1;
                for (int i = 0; i < quantity; i++)
                {
                    await InsertAsync(item);
                    player.Character.Inventory.Quantity++;
                    player.Character.Inventory.QuantityDifferentItens++;
                }
                return;
            }
            var itemFound = await FindAsync(item.ItemID, player);
            if (itemFound == null)
            {
                item.Quantity = quantity;
                await InsertAsync(item);
                player.Character.Inventory.Quantity += quantity;
                player.Character.Inventory.QuantityDifferentItens++;
                return;
            }

            itemFound.Quantity += quantity;
            await ReplaceAsync(itemFound);
            player.Character.Inventory.Quantity += quantity;
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
        public ulong TargetId;
        public string Message = null;
        public Response(DiscordEmbedBuilder embed) => Embed = embed;
        public Response(string message) => Message = message;
    }
}
