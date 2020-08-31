using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class Wiki
    {
        [BsonId]
        public string Id { get; set; }
        public string Nome { get; set; }
        public List<string> Texto { get; set; }
    }
}
