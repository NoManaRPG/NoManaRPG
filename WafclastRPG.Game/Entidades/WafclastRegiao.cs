using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;
using WafclastRPG.Game.Entidades.NPC;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Entidades
{
    public class WafclastRegiao
    {
        /* Regions?
        * 
        * Regions are where is the character is
        * Every region has some monster or npc, that the player can interact
        * Some exit to other region.
        */

        public int Id { get; private set; }
        public RegiaoType Reino { get; private set; }
        public string Local { get; private set; }

        public List<WafclastMonstro> Monstros { get; set; } = new List<WafclastMonstro>();
        public List<WafclastNPC> NPCs { get; set; } = new List<WafclastNPC>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<SaidaType, int> Saidas { get; set; } = new Dictionary<SaidaType, int>();

        public WafclastRegiao(int id, RegiaoType reino, string local)
        {
            Id = id;
            Reino = reino;
            Local = local;
        }
    }
}
