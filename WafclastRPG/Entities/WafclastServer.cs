using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities {
  [BsonIgnoreExtraElements]
  public class WafclastServer {
    [BsonId]
    public ulong Id { get; private set; }
    public string Prefix { get; private set; }

    public WafclastServer(ulong id) {
      Id = id;
    }

    public void SetPrefix(string prefix) => this.Prefix = prefix;
  }
}
