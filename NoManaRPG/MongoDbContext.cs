// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using NoManaRPG.Extensions;
using NoManaRPG;
using NoManaRPG.Entities;
using NoManaRPG.Entities.Items;
using NoManaRPG.Entities.Rooms;

namespace NoManaRPG;

public class MongoDbContext
{

    public IMongoClient Client { get; }
    public IMongoDatabase Database { get; }

    public IMongoCollection<Player> Players { get; }
    public IMongoCollection<Room> Zones { get; }
    public IMongoCollection<Item> Items { get; }

    public IMongoCollection<Server> Servers { get; }
    public IMongoCollection<RankUpgrader> Upgraders { get; }

    public UsersBlocked UsersTemporaryBlocked { get; private set; }

    public MongoDbContext(string connection)
    {

        var mongoUrl = new MongoUrl(connection);
        var dbName = mongoUrl.DatabaseName;

        this.Client = new MongoClient(mongoUrl);
        this.Database = this.Client.GetDatabase(dbName);

        this.Players = this.Database.CreateCollection<Player>("WafclastPlayers");
        this.Servers = this.Database.CreateCollection<Server>("WafclastServers");
        this.Items = this.Database.CreateCollection<Item>("WafclastItems");
        this.Zones = this.Database.CreateCollection<Room>("WafclastZones");
        this.Upgraders = this.Database.CreateCollection<RankUpgrader>("WafclastRankUpgraders");

        #region Usar no futuro
        //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
        //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
        //ColecaoJogador.Indexes.CreateOne(indexModel);
        #endregion
    }


    public async Task<string> GetServerPrefixAsync(ulong serverId, string defaultPrefix)
    {
        var svl = await this.Servers.Find(x => x.Id == serverId).FirstOrDefaultAsync();
        if (svl == null)
            return defaultPrefix;
        return svl.Prefix;
    }
    public string GetServerPrefix(ulong serverId, string defaultPrefix)
    {
        var svl = this.Servers.Find(x => x.Id == serverId).FirstOrDefault();
        if (svl == null)
            return defaultPrefix;
        return svl.Prefix;
    }
    public Task DeleteServerAsync(ulong serverId)
        => this.Servers.DeleteOneAsync(x => x.Id == serverId);
}
