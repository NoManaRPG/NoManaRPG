using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;
using WafclastRPG.Extensions;

namespace WafclastRPG.DataBases
{
    public class Database
    {
        public IMongoClient MongoClient { get; }
        public IMongoDatabase MongoDatabase { get; }
        public IMongoCollection<WafclastPlayer> CollectionJogadores { get; }
        public IMongoCollection<WafclastServer> CollectionServidores { get; }
        public IMongoCollection<WafclastMonster> CollectionMonsters { get; }
        public IMongoCollection<WafclastMapa> CollectionMaps { get; }
        public IMongoCollection<WafclastBaseItem> CollectionItens { get; }

        public ConcurrentDictionary<ulong, bool> PrefixLocker { get; }

        public Database()
        {
            #region Connection string
            MongoClient = new MongoClient("mongodb://localhost?retryWrites=true");
#if DEBUG
            MongoDatabase = MongoClient.GetDatabase("WafclastV2Debug");
#else
            MongoDatabase = MongoClient.GetDatabase("WafclastV2");
#endif
            #endregion

            WafclastCharacter.MapBuilder();
            WafclastCoins.MapBuilder();
            WafclastLevel.MapBuilderLevel();
            WafclastMapa.MapBuilder();
            WafclastMonster.MapBuilder();
            WafclastPlayer.MapBuilder();
            WafclastBaseItem.MapBuilder();

            CollectionJogadores = MongoDatabase.CriarCollection<WafclastPlayer>();
            CollectionServidores = MongoDatabase.CriarCollection<WafclastServer>();
            CollectionMonsters = MongoDatabase.CriarCollection<WafclastMonster>();
            CollectionMaps = MongoDatabase.CriarCollection<WafclastMapa>();
            CollectionItens = MongoDatabase.CriarCollection<WafclastBaseItem>();
            PrefixLocker = new ConcurrentDictionary<ulong, bool>();

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public async Task<DatabaseSession> StartDatabaseSessionAsync() => new DatabaseSession(
            await MongoClient.StartSessionAsync(), this);

        #region Player

        /// <summary>
        /// Procura no banco de dados pelo o Id informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public Task<WafclastPlayer> FindPlayerAsync(ulong id)
            => CollectionJogadores.Find(x => x.Id == id).FirstOrDefaultAsync();

        /// <summary>
        /// Procura no banco de dados pelo o DiscordUser informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O Jogador ou null</returns>
        public Task<WafclastPlayer> FindPlayerAsync(DiscordUser user)
            => CollectionJogadores.Find(x => x.Id == user.Id).FirstOrDefaultAsync();

        /// <summary>
        /// Procura no banco de dados pelo o Id informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public Task<WafclastPlayer> FindPlayerAsync(CommandContext ctx)
            => CollectionJogadores.Find(x => x.Id == ctx.User.Id).FirstOrDefaultAsync();

        #endregion
        #region Monster

        public Task<WafclastMonster> FindMonsterAsync(string id)
            => CollectionMonsters.Find(x => x.Id == id).FirstOrDefaultAsync();

        #endregion
        #region Interactivity

        public bool IsExecutingInteractivity(ulong userId) => PrefixLocker.TryGetValue(userId, out _);
        public void StopExecutingInteractivity(ulong userId) => PrefixLocker.TryRemove(userId, out _);
        public void StopExecutingInteractivity(CommandContext ctx) => PrefixLocker.TryRemove(ctx.User.Id, out _);
        public void StartExecutingInteractivity(ulong userId) => PrefixLocker.TryAdd(userId, true);
        public void StartExecutingInteractivity(CommandContext ctx) => PrefixLocker.TryAdd(ctx.User.Id, true);

        #endregion
        #region Server

        public async Task<string> GetServerPrefixAsync(ulong serverId, string defaultPrefix)
        {
            var svl = await CollectionServidores.Find(x => x.Id == serverId).FirstOrDefaultAsync();
            if (svl == null)
                return defaultPrefix;
            return svl.Prefix;
        }
        public string GetServerPrefix(ulong serverId, string defaultPrefix)
        {
            var svl = CollectionServidores.Find(x => x.Id == serverId).FirstOrDefault();
            if (svl == null)
                return defaultPrefix;
            return svl.Prefix;
        }
        public Task DeleteServerAsync(ulong serverId)
            => CollectionServidores.DeleteOneAsync(x => x.Id == serverId);
        public Task ReplaceServerAsync(ulong serverId, WafclastServer server)
            => CollectionServidores.ReplaceOneAsync(x => x.Id == serverId, server, new ReplaceOptions { IsUpsert = true });

        #endregion
        #region Map

        public Task<WafclastMapa> FindMapAsync(ulong id)
            => CollectionMaps.Find(x => x.Id == id).FirstOrDefaultAsync();

        public Task<WafclastMapa> FindMapAsync(CommandContext ctx)
          => CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();

        #endregion
        #region Itens

        public Task<WafclastBaseItem> FindItemByItemIdAsync(ulong itemId, ulong playerId)
          => CollectionItens.Find(x => x.PlayerId == playerId && x.ItemID == itemId).FirstOrDefaultAsync();

        public Task<WafclastBaseItem> FindItemByObjectIdAsync(ObjectId id, ulong playerId)
        => CollectionItens.Find(x => x.Id == id).FirstOrDefaultAsync();

        public Task InsertItemAsync(WafclastBaseItem item)
          => CollectionItens.InsertOneAsync(item);

        public async Task<ulong> FindLastItem(CommandContext ctx)
        {
            var item = await CollectionItens.Find(x => x.PlayerId == ctx.Guild.Id).SortByDescending(x => x.ItemID).Limit(1).FirstOrDefaultAsync();
            if (item == null)
                return 1;
            return item.ItemID + 1;
        }
        #endregion
    }
}
