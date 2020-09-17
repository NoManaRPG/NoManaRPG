using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace TorreRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class MonstroNomes
    {

        public List<string> Nomes { get; set; }
        public List<string> Chefoes { get; set; }

        public static Dictionary<int, MonstroNomes> GetMonstros()
        {
            Dictionary<int, MonstroNomes> lista = new Dictionary<int, MonstroNomes>();
            lista.Add(1, new MonstroNomes { Nomes = new List<string> { "Afogado", "Cadáver Faminto", "Cuspidor da Areia" } });
            lista.Add(2, new MonstroNomes { Nomes = new List<string> { "Fúria Ardente", "Canibal", "Devorador de Cascalho", "Caranguejo Selvagem", "Pele Murcha" } });
            lista.Add(3, new MonstroNomes { Nomes = new List<string> { "Canibal", "Afogado", "Caranguejo Selvagem", "Andarilho da Maré", "Hailrake" } });
            lista.Add(4, new MonstroNomes { Nomes = new List<string> { "Costa-limo Fedido", "Morto Encharcado", "Grande Rhoa", "Rhoa Carniceiro", "Andarilho da Maré" } });
            lista.Add(5, new MonstroNomes { Nomes = new List<string> { "Kadavrus, o Profanador", "Rhoa de Ossos", "Morto Encharcado", "Besta Esqueletal" } });

            return lista;
        }
    }
}
