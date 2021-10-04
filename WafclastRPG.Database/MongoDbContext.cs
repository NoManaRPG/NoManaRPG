// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Extensions;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Itens;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database
{
    public class MongoDbContext
    {

        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public IMongoCollection<WafclastPlayer> Players { get; }
        public IMongoCollection<WafclastRoom> Rooms { get; }
        public IMongoCollection<WafclastBaseItem> Items { get; }

        public IMongoCollection<WafclastServer> Servers { get; }
        public IMongoCollection<WafclastFabrication> Fabrications { get; }

        public MongoDbContext(string connection)
        {

            var mongoUrl = new MongoUrl(connection);
            var dbName = mongoUrl.DatabaseName;

            this.Client = new MongoClient(mongoUrl);
            this.Database = this.Client.GetDatabase(dbName);

            this.Players = this.Database.CreateCollection<WafclastPlayer>("WafclastPlayers");
            this.Servers = this.Database.CreateCollection<WafclastServer>("WafclastServers");
            this.Items = this.Database.CreateCollection<WafclastBaseItem>("WafclastItems");
            this.Rooms = this.Database.CreateCollection<WafclastRoom>("WafclastRooms");


            this.Fabrications = this.Database.CreateCollection<WafclastFabrication>("WafclastRecipes");

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
}
