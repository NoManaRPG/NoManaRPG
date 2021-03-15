using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WafclastRPG.Bot.Entidades;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Database
{
    public class BotDatabase
    {
        public IMongoClient MongoClient { get; }
        public IMongoDatabase MongoDatabase { get; }
        public IMongoCollection<WafclastPlayer> CollectionJogadores { get; }
        public IMongoCollection<Server> CollectionServidores { get; }
        public IMongoCollection<WafclastMonster> CollectionMonsters { get; }
        public IMongoCollection<WafclastMapa> CollectionMaps { get; }

        public ConcurrentDictionary<ulong, bool> PrefixLocker
        { get; }

        public BotDatabase()
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

            CollectionJogadores = MongoDatabase.CriarCollection<WafclastPlayer>();
            CollectionServidores = MongoDatabase.CriarCollection<Server>();
            CollectionMonsters = MongoDatabase.CriarCollection<WafclastMonster>();
            CollectionMaps = MongoDatabase.CriarCollection<WafclastMapa>();
            PrefixLocker = new ConcurrentDictionary<ulong, bool>();

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public async Task<BotDatabaseSession> StartDatabaseSessionAsync() => new BotDatabaseSession(
            await MongoClient.StartSessionAsync(), this);

        /// <summary>
        /// Procura no banco de dados pelo o Id informado.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>O BotJogador ou null</returns>
        public async Task<WafclastPlayer> FindPlayerAsync(ulong id)
            => await CollectionJogadores.Find(x => x.Id == id).FirstOrDefaultAsync();

        #region Interactivity

        public bool IsExecutingInteractivity(ulong userId) => PrefixLocker.TryGetValue(userId, out _);
        public void StopExecutingInteractivity(ulong userId) => PrefixLocker.TryRemove(userId, out _);
        public void StartExecutingInteractivity(ulong userId) => PrefixLocker.TryAdd(userId, true);

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
        public Task ReplaceServerAsync(ulong serverId, Server server)
            => CollectionServidores.ReplaceOneAsync(x => x.Id == serverId, server, new ReplaceOptions { IsUpsert = true });

        #endregion

    }
}
