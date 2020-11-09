using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Bot.Entidades
{
    [BsonIgnoreExtraElements]
    public class BotServidor
    {
        [BsonId]
        public ulong Id { get; private set; }
        public string Prefix { get; private set; }

        public BotServidor(ulong id)
        {
            this.Id = id;
        }

        public void SetPrefix(string prefix) => this.Prefix = prefix;
    }
}
