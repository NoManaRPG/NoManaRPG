using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;


namespace WafclastRPG.Context {
  public class MongoDbContext {

    public IMongoClient Client { get; }
    public IMongoDatabase Database { get; }

    public IMongoCollection<Player> Players { get; }
    public IMongoCollection<Room> Rooms { get; }
    public IMongoCollection<WafclastBaseItem> Items { get; }

    public IMongoCollection<WafclastServer> Servers { get; }
    public IMongoCollection<WafclastFabrication> Fabrications { get; }

    public MongoDbContext() {
      #region Connection string
      Client = new MongoClient("mongodb://localhost?retryWrites=true");
#if DEBUG
      Database = Client.GetDatabase("WafclastDebug");
#else
            Database = Client.GetDatabase("Wafclast");
#endif
      #endregion

      Players = Database.CreateCollection<Player>();
      Servers = Database.CreateCollection<WafclastServer>();
      Items = Database.CreateCollection<WafclastBaseItem>();
      Rooms = Database.CreateCollection<Room>();


      Fabrications = Database.CreateCollection<WafclastFabrication>();

      #region Usar no futuro
      //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
      //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
      //ColecaoJogador.Indexes.CreateOne(indexModel);
      #endregion
    }


    public async Task<string> GetServerPrefixAsync(ulong serverId, string defaultPrefix) {
      var svl = await Servers.Find(x => x.Id == serverId).FirstOrDefaultAsync();
      if (svl == null)
        return defaultPrefix;
      return svl.Prefix;
    }
    public string GetServerPrefix(ulong serverId, string defaultPrefix) {
      var svl = Servers.Find(x => x.Id == serverId).FirstOrDefault();
      if (svl == null)
        return defaultPrefix;
      return svl.Prefix;
    }
    public Task DeleteServerAsync(ulong serverId)
        => Servers.DeleteOneAsync(x => x.Id == serverId);
  }
}
