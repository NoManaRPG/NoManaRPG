using MongoDB.Bson;
using MongoDB.Driver;

namespace WafclastRPG.Extensions
{
    public static class IMongoDatabaseExtension
    {
        public static IMongoCollection<T> CriarCollection<T>(this IMongoDatabase database, CreateCollectionOptions createCollectionOptions = null)
        {
            var filtro = new ListCollectionNamesOptions { Filter = Builders<BsonDocument>.Filter.Eq("name", typeof(T).Name) };
            if (!database.ListCollectionNames(filtro).Any()) database.CreateCollection(typeof(T).Name, createCollectionOptions);
            return database.GetCollection<T>(typeof(T).Name);
        }
    }
}
