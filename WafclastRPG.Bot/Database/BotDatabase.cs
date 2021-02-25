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
        private IMongoClient MongoClient { get; }
        private IMongoDatabase MongoDatabase { get; }
        private IMongoCollection<WafclastPlayer> CollectionJogadores { get; }
        private IMongoCollection<BotServidor> CollectionServidores { get; }

        private ConcurrentDictionary<ulong, bool> PrefixLocker { get; }

        public BotDatabase()
        {
            #region Connection string
            MongoClient = new MongoClient("mongodb://localhost?retryWrites=true");
#if DEBUG
            MongoDatabase = MongoClient.GetDatabase("WafclastV2Debug");
#else
            Database = Client.GetDatabase("WafclastV2");
#endif
            #endregion

            WafclastPlayer.MapBuilder();
            WafclastLevel.MapBuilderLevel();
            WafclastCoins.MapBuilder();
            WafclastCharacter.MapBuilder();

            CollectionJogadores = MongoDatabase.CriarCollection<WafclastPlayer>();
            CollectionServidores = MongoDatabase.CriarCollection<BotServidor>();
            PrefixLocker = new ConcurrentDictionary<ulong, bool>();

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public async Task<BotDatabaseSession> StartDatabaseSessionAsync() => new BotDatabaseSession(
            await MongoClient.StartSessionAsync(),
            CollectionJogadores);

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
        public Task ReplaceServerAsync(ulong serverId, BotServidor server)
            => CollectionServidores.ReplaceOneAsync(x => x.Id == serverId, server, new ReplaceOptions { IsUpsert = true });

        #endregion

    }
}
