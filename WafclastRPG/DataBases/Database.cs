using DSharpPlus.CommandsNext;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.MercadoGeral;
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
        public IMongoCollection<WafclastBaseItem> CollectionItems { get; }
        public IMongoCollection<Ordem> CollectionOrdens { get; }

        public ConcurrentDictionary<ulong, bool> InteractivityLocker { get; }

        public static List<WafclastOreItem> MineDrop = new List<WafclastOreItem>();

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

            CollectionPlayers = Database.CriarCollection<WafclastPlayer>();
            CollectionGuilds = Database.CriarCollection<WafclastServer>();
            CollectionMonsters = Database.CriarCollection<WafclastMonster>();
            CollectionItems = Database.CriarCollection<WafclastBaseItem>();
            CollectionOrdens = Database.CriarCollection<Ordem>();

            InteractivityLocker = new ConcurrentDictionary<ulong, bool>();

            #region Usar no futuro
            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            #endregion
        }

        public void MineDropBuilder()
        {
            MineDrop.Add(new WafclastOreItem(new WafclastBaseItem
            {
                Name = "Argila",
                Description = "Um pouco de argila seca e dura."
            })
            {
                Hardness = 0,
                DropChance = 50d / 100d,
                MinLevel = 5,
                Type = WafclastOreItem.OreType.Clay,
                ExperienceGain = 5
            });
        }

        public async Task<DatabaseSession> StartDatabaseSessionAsync()
            => new DatabaseSession(await Client.StartSessionAsync(), this);

        public bool IsExecutingInteractivity(ulong userId) => InteractivityLocker.TryGetValue(userId, out _);
        public void StopExecutingInteractivity(ulong userId) => InteractivityLocker.TryRemove(userId, out _);
        public void StopExecutingInteractivity(CommandContext ctx) => InteractivityLocker.TryRemove(ctx.User.Id, out _);
        public void StartExecutingInteractivity(ulong userId) => InteractivityLocker.TryAdd(userId, true);
        public void StartExecutingInteractivity(CommandContext ctx) => InteractivityLocker.TryAdd(ctx.User.Id, true);

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
