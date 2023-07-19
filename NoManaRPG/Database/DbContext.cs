// This file is part of NoManaRPG project.

using MongoDB.Driver;
using NoManaRPG.Entidades;
using NoManaRPG.Entidades.Items;
using NoManaRPG.Entidades.Rooms;
using NoManaRPG.Extensions;

namespace NoManaRPG.Database;

public class DbContext
{

    public IMongoClient Client { get; }
    public IMongoDatabase Database { get; }

    public IMongoCollection<Jogador> Players { get; }
    public IMongoCollection<Room> Zones { get; }
    public IMongoCollection<Item> Items { get; }

    public DbContext(string connection)
    {

        var mongoUrl = new MongoUrl(connection);
        var dbName = mongoUrl.DatabaseName;

        this.Client = new MongoClient(mongoUrl);
        this.Database = this.Client.GetDatabase(dbName);

        this.Players = this.Database.CreateCollection<Jogador>("NoManaPlayers");
        this.Items = this.Database.CreateCollection<Item>("NoManaItems");
        this.Zones = this.Database.CreateCollection<Room>("NoManaZones");

        #region Usar no futuro
        //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
        //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
        //ColecaoJogador.Indexes.CreateOne(indexModel);
        #endregion
    }
}
