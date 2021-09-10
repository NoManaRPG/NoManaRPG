using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;

namespace WafclastRPG.Entities {
  public enum ExitType {
    Norte = 0,
    Sul = 1,
    Leste = 2,
    Oeste = 3,
    Entrar = 4,
    Sair = 5,
    Subir = 6,
    Descer = 7,
  }

  [BsonIgnoreExtraElements]
  public class WafclastRegion {

    [BsonId]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public WafclastMonster Monster { get; set; }
    public double PlayerAttackSpeedPoints { get; set; }
    public double MonsterAttackSpeedPoints { get; set; }
    public double TotalAttackSpeedPoints { get; set; }


    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<ExitType, int> Exits { get; set; }

    public WafclastRegion(int id, string local) {
      Id = id;
      Name = local;

      Exits = new Dictionary<ExitType, int>();
    }
  }
}
