using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Globalization;

namespace WafclastRPG.Entities.Wafclast {
  [BsonIgnoreExtraElements]
  public class Player {
    [BsonId]
    public ulong Id { get; private set; }
    public DateTime DateAccountCreation { get; private set; }
    public BaseCharacter Character { get; private set; }

    public ulong MonsterKills { get; set; }
    public ulong Deaths { get; set; }

    public string Language { get; set; } = "pt-BR";

    public Player(ulong id, BaseCharacter character) {
      this.Id = id;
      this.Character = character;
      DateAccountCreation = DateTime.UtcNow;
    }

    public string Mention { get => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>"; }
  }
}
