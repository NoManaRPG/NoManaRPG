using MongoDB.Bson;
using MongoDB.Driver;

namespace TorreRPG.Extensoes
{
    public static class IMongoDatabaseExtension
    {
        public static IMongoCollection<T> CriarCollection<T>(this IMongoDatabase database)
        {
            var filtro = new ListCollectionNamesOptions { Filter = Builders<BsonDocument>.Filter.Eq("name", typeof(T).Name) };
            if (!database.ListCollectionNames(filtro).Any()) database.CreateCollection(typeof(T).Name);
            return database.GetCollection<T>(typeof(T).Name);
        }
    }
}
