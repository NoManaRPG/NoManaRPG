using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Maps;
using WafclastRPG.Entities.Monsters;
using WafclastRPG.Extensions;

namespace WafclastRPG.DataBases
{
    public class DataBase
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public IMongoCollection<WafclastPlayer> CollectionPlayers { get; }
        public IMongoCollection<WafclastServer> CollectionGuilds { get; }
        public IMongoCollection<WafclastMonster> CollectionMonsters { get; }
        public IMongoCollection<WafclastMap> CollectionMaps { get; }
        public IMongoCollection<WafclastBaseItem> CollectionItems { get; }

        public ConcurrentDictionary<ulong, bool> InteractivityLocker { get; }

        public DataBase()
        {
            #region Connection string
            Client = new MongoClient("mongodb://localhost?retryWrites=true");
#if DEBUG
            Database = Client.GetDatabase("WafclastV2Debug");
#else
            Database = Client.GetDatabase("WafclastV2");
#endif
            #endregion

            WafclastCharacter.MapBuilder();
            WafclastCoins.MapBuilder();
            WafclastLevel.MapBuilderLevel();
            WafclastMap.MapBuilder();
            WafclastMonsterBase.MapBuilder();
            WafclastPlayer.MapBuilder();
            WafclastBaseItem.MapBuilder();

            CollectionPlayers = Database.CriarCollection<WafclastPlayer>();
            CollectionGuilds = Database.CriarCollection<WafclastServer>();
            CollectionMonsters = Database.CriarCollection<WafclastMonster>();
            CollectionMaps = Database.CriarCollection<WafclastMap>();
            CollectionItems = Database.CriarCollection<WafclastBaseItem>();

            InteractivityLocker = new ConcurrentDictionary<ulong, bool>();

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public async Task<DatabaseSession> StartDatabaseSessionAsync()
            => new DatabaseSession(await Client.StartSessionAsync(), this);

        #region FindAsync


        public Task<WafclastPlayer> FindAsync(DiscordUser user)
          => CollectionPlayers.Find(x => x.Id == user.Id).FirstOrDefaultAsync();
        public Task<WafclastMonster> FindAsync(WafclastMonsterBase monster)
          => CollectionMonsters.Find(x => x.Id == monster.Id).FirstOrDefaultAsync();
        public Task<WafclastMap> FindAsync(WafclastLocalization localization)
          => CollectionMaps.Find(x => x.Id == localization.ChannelId).FirstOrDefaultAsync();
        public Task<WafclastMap> FindAsync(DiscordChannel discordChannel)
          => CollectionMaps.Find(x => x.Id == discordChannel.Id).FirstOrDefaultAsync();

        /// <summary>
        /// Procura o ID + 1 do último item feito na Guilda.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns>Retorna 1 caso não encontre nenhum item feito na Guilda.</returns>
        public async Task<ulong> GetLastIDAsync(DiscordGuild guild)
        {
            var item = await CollectionItems.Find(x => x.PlayerId == guild.Id).SortByDescending(x => x.ItemID).Limit(1).FirstOrDefaultAsync();
            if (item == null)
                return 1;
            return item.ItemID + 1;
        }
        public Task<WafclastBaseItem> FindAsync(ObjectId id)
          => CollectionItems.Find(x => x.Id == id).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindAsync(ulong itemId, DiscordGuild guild)
        => CollectionItems.Find(x => x.PlayerId == guild.Id && x.ItemID == guild.Id).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindAsync(ulong itemId, WafclastPlayer player)
        => CollectionItems.Find(x => x.PlayerId == player.Id && x.ItemID == itemId).FirstOrDefaultAsync();
        public Task<WafclastBaseItem> FindAsync(string itemName, WafclastPlayer player)
                => CollectionItems.Find(x => x.PlayerId == player.Id && x.Name == itemName,
                    new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();


        #endregion
        #region Replace


        public Task ReplaceAsync(WafclastPlayer jogador)
          => CollectionPlayers.ReplaceOneAsync(x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastServer server)
          => CollectionGuilds.ReplaceOneAsync(x => x.Id == server.Id, server, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastBaseItem item)
         => CollectionItems.ReplaceOneAsync(x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastMap map)
            => CollectionMaps.ReplaceOneAsync(x => x.Id == map.Id, map, new ReplaceOptions { IsUpsert = true });
        public Task ReplaceAsync(WafclastMonster monster)
          => CollectionMonsters.ReplaceOneAsync(x => x.Id == monster.Id, monster, new ReplaceOptions { IsUpsert = true });

        #endregion
        #region Insert


        public async Task InsertAsync(WafclastBaseItem item)
        {
            item.Id = ObjectId.Empty;
            await CollectionItems.InsertOneAsync(item);
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
        #region Interactivity

        public bool IsExecutingInteractivity(ulong userId) => InteractivityLocker.TryGetValue(userId, out _);
        public void StopExecutingInteractivity(ulong userId) => InteractivityLocker.TryRemove(userId, out _);
        public void StopExecutingInteractivity(CommandContext ctx) => InteractivityLocker.TryRemove(ctx.User.Id, out _);
        public void StartExecutingInteractivity(ulong userId) => InteractivityLocker.TryAdd(userId, true);
        public void StartExecutingInteractivity(CommandContext ctx) => InteractivityLocker.TryAdd(ctx.User.Id, true);

        #endregion

        public async Task<string> GetServerPrefixAsync(ulong serverId, string defaultPrefix)
        {
            var svl = await CollectionGuilds.Find(x => x.Id == serverId).FirstOrDefaultAsync();
            if (svl == null)
                return defaultPrefix;
            return svl.Prefix;
        }
        public string GetServerPrefix(ulong serverId, string defaultPrefix)
        {
            var svl = CollectionGuilds.Find(x => x.Id == serverId).FirstOrDefault();
            if (svl == null)
                return defaultPrefix;
            return svl.Prefix;
        }
        public Task DeleteServerAsync(ulong serverId)
            => CollectionGuilds.DeleteOneAsync(x => x.Id == serverId);
    }
}
