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
            lista.Add(1, new MonstroNomes
            {
                Nomes = new List<string> {
                "Afogado", "Destruidor",},
                Chefoes = new List<string> { "Demolidor" }
            });

            return lista;
        }
    }
}
