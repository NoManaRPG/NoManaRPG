using DragonsDiscordRPG.Game.Entidades;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG.Entidades
{
    public class ModuloBanco
    {
        public static IMongoDatabase Database { get; private set; }
        public static IMongoCollection<Jogador> ColecaoJogador { get; private set; }
        public static IMongoClient Cliente { get; private set; }

        public ModuloBanco()
        {
            //IMongoClient _client = new MongoClient("mongodb://admin:.Sts8562@localhost");
            Cliente = new MongoClient();
            Database = Cliente.GetDatabase("zaynbot");
            ColecaoJogador = Database.GetCollection<Jogador>("Jogador");

            //BsonSerializer.RegisterSerializer(typeof(float),
            //    new SingleSerializer(BsonType.Double, new RepresentationConverter(
            //    true, //allow truncation
            //    true // allow overflow, return decimal.MinValue or decimal.MaxValue instead
            //)));


            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
            var filter = new ListCollectionNamesOptions { Filter = Builders<BsonDocument>.Filter.Eq("name", nameof(Jogador)) };
            if (!Database.ListCollectionNames(filter).Any())
                Database.CreateCollection(nameof(Jogador));
        }

        public static void EditJogador(Jogador jogador) => ColecaoJogador.ReplaceOne(x => x.Id == jogador.Id, jogador);
    }
}
