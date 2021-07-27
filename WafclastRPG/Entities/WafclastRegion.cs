using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.Entities
{
    public enum ExitType
    {
        Norte = 0,
        Sul = 1,
        Leste = 2,
        Oeste = 3,
        Entrar = 4,
        Sair = 5,
        Subir = 6,
        Descer
    }

    public class WafclastRegion
    {
        /* Regions
        * 
        * Regions are where is the character is
        * Every region has some monster or npc, that the player can interact
        * Some exit to other region.
        */

        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<WafclastMonster> Monsters { get; set; } = new List<WafclastMonster>();
        //public List<WafclastNPC> NPCs { get; set; } = new List<WafclastNPC>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<ExitType, int> Exits { get; set; } = new Dictionary<ExitType, int>();

        public WafclastRegion(int id, string local)
        {
            Id = id;
            Name = local;
        }
    }
}
