// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Extensions;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Itens;
using WafclastRPG.Game.Entities.Wafclast;

namespace WafclastRPG.Database
{
    public class MongoDbContext
    {

        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public IMongoCollection<Player> Players { get; }
        public IMongoCollection<Room> Rooms { get; }
        public IMongoCollection<WafclastBaseItem> Items { get; }

        public IMongoCollection<WafclastServer> Servers { get; }
        public IMongoCollection<WafclastFabrication> Fabrications { get; }

        public MongoDbContext()
        {
            #region Connection string
            this.Client = new MongoClient("mongodb://localhost?retryWrites=true");
#if DEBUG
            this.Database = this.Client.GetDatabase("WafclastDebug");
#else
            Database = Client.GetDatabase("Wafclast");
#endif
            #endregion

            this.Players = this.Database.CreateCollection<Player>();
            this.Servers = this.Database.CreateCollection<WafclastServer>();
            this.Items = this.Database.CreateCollection<WafclastBaseItem>();
            this.Rooms = this.Database.CreateCollection<Room>();


            this.Fabrications = this.Database.CreateCollection<WafclastFabrication>();

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
