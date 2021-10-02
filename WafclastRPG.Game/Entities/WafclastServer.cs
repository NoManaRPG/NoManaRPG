// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastServer
    {
        [BsonId]
        public ulong Id { get; private set; }
        public string Prefix { get; private set; }

        public WafclastServer(ulong id)
        {
            this.Id = id;
        }

        public void SetPrefix(string prefix) => this.Prefix = prefix;
    }
}
