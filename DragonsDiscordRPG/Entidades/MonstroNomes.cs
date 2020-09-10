using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class MonstroNomes
    {

        public List<string> Nomes { get; set; }
        public List<string> Chefoes { get; set; }

        public static Dictionary<int, MonstroNomes> GetMonstros()
        {
            Dictionary<int, MonstroNomes> lista = new Dictionary<int, MonstroNomes>();
            lista.Add(1, new MonstroNomes { Nomes = new List<string> { "Zombie afogado", "Zombie faminto", "Cuspidor de areia" } });
            lista.Add(2, new MonstroNomes { Nomes = new List<string> { "Fogo furioso", "Canibal", "Comedor de terra", "Carangueiro gigante","Casca murcha" } });

            return lista;
        }
    }
}
