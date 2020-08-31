using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonsDiscordRPG
{
    public static class ModuloBanco
    {
        public static IMongoClient Cliente { get; private set; }
        public static IMongoDatabase Database { get; private set; }
        public static IMongoCollection<Jogador> ColecaoJogador { get; private set; }
        public static IMongoCollection<Regiao> ColecaoRegiao { get; private set; }

        public static void Conectar()
        {
            Cliente = new MongoClient();
            Database = Cliente.GetDatabase("Dragon");

            ColecaoJogador = Database.CriarCollection<Jogador>();
            ColecaoRegiao = Database.CriarCollection<Regiao>();

            //BsonSerializer.RegisterSerializer(typeof(float),
            //    new SingleSerializer(BsonType.Double, new RepresentationConverter(
            //    true, //allow truncation
            //    true // allow overflow, return decimal.MinValue or decimal.MaxValue instead
            //)));


            //var notificationLogBuilder = Builders<RPGJogador>.IndexKeys;
            //var indexModel = new CreateIndexModel<RPGJogador>(notificationLogBuilder.Ascending(x => x.NivelAtual));
            //ColecaoJogador.Indexes.CreateOne(indexModel);
        }
    }
}
