using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public class Server
    {
        [BsonId]
        public ulong Id { get; private set; }
        public string Prefix { get; private set; }

        public Server(ulong id)
        {
            this.Id = id;
        }

        public void SetPrefix(string prefix) => this.Prefix = prefix;
    }
}
