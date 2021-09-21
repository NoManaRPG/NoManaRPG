using DSharpPlus.CommandsNext;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;

namespace WafclastRPG.DataBases {
  public class DataBase {
    public IMongoClient Client { get; }
    public IMongoDatabase Database { get; }

    public IMongoCollection<Player> CollectionPlayers { get; }
    public IMongoCollection<WafclastServer> CollectionGuilds { get; }
    public IMongoCollection<WafclastBaseItem> CollectionItems { get; }
    public IMongoCollection<Room> CollectionRooms { get; }


    public IMongoCollection<WafclastFabrication> CollectionFabrication { get; }

    public ConcurrentDictionary<ulong, bool> InteractivityLocker { get; }

    public DataBase() {
      #region Connection string
      Client = new MongoClient("mongodb://localhost?retryWrites=true");
#if DEBUG
      Database = Client.GetDatabase("WafclastDebug");
#else
            Database = Client.GetDatabase("Wafclast");
#endif
      #endregion

      CollectionPlayers = Database.CreateCollection<Player>();
      CollectionGuilds = Database.CreateCollection<WafclastServer>();
      CollectionItems = Database.CreateCollection<WafclastBaseItem>();
      CollectionRooms = Database.CreateCollection<Room>();


      CollectionFabrication = Database.CreateCollection<WafclastFabrication>();

      InteractivityLocker = new ConcurrentDictionary<ulong, bool>();

      #region Usar no futuro
      //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
      //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
      //ColecaoJogador.Indexes.CreateOne(indexModel);
      #endregion
    }


    public async Task<DatabaseSession> StartDatabaseSessionAsync()
        => new DatabaseSession(await Client.StartSessionAsync(), this);

    #region Interactivity
    public bool IsExecutingInteractivity(ulong userId) => InteractivityLocker.TryGetValue(userId, out _);
    public void StopExecutingInteractivity(ulong userId) => InteractivityLocker.TryRemove(userId, out _);
    public void StopExecutingInteractivity(CommandContext ctx) => InteractivityLocker.TryRemove(ctx.User.Id, out _);
    public void StartExecutingInteractivity(ulong userId) => InteractivityLocker.TryAdd(userId, true);
    public void StartExecutingInteractivity(CommandContext ctx) => InteractivityLocker.TryAdd(ctx.User.Id, true);
    #endregion

    public async Task<string> GetServerPrefixAsync(ulong serverId, string defaultPrefix) {
      var svl = await CollectionGuilds.Find(x => x.Id == serverId).FirstOrDefaultAsync();
      if (svl == null)
        return defaultPrefix;
      return svl.Prefix;
    }
    public string GetServerPrefix(ulong serverId, string defaultPrefix) {
      var svl = CollectionGuilds.Find(x => x.Id == serverId).FirstOrDefault();
      if (svl == null)
        return defaultPrefix;
      return svl.Prefix;
    }
    public Task DeleteServerAsync(ulong serverId)
        => CollectionGuilds.DeleteOneAsync(x => x.Id == serverId);

    public async Task ReloadItemsAsync(ulong botId) {
      await CollectionItems.DeleteManyAsync(x => x.PlayerId == botId);

      object classInstance = Activator.CreateInstance(typeof(DatabaseItems), null);
      var itens = typeof(DatabaseItems).GetMethods();
      for (int i = 0; i < itens.Length - 4; i++) {
        var reg = (WafclastBaseItem) itens[i].Invoke(classInstance, null);
        await CollectionItems.InsertOneAsync(reg);
      }
    }
  }
}
