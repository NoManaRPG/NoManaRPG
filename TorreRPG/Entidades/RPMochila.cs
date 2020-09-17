using TorreRPG.Entidades.Itens;
using TorreRPG.Extensoes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace TorreRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPMochila
    {
        public List<RPItem> Itens { get; set; }

        public int Espaco { get; set; } // Max 64

        public RPMochila()
        {
            Itens = new List<RPItem>();
        }

        public bool TryAddItem(RPItem item, int quantidade = 1)
        {
            if (Espaco + (item.Espaco * quantidade) <= 64)
            {
                Itens.Add(item);
                Espaco += item.Espaco * quantidade;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retorna null caso não tenha a quantidade especificada.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        public bool TryRemoveItem(int index, out RPItem item, int quantidade = 1)
        {
            var im = Itens.ElementAtOrDefault(index);
            item = null;
            if (im != null)
            {
                switch (im.Classe)
                {
                    default:
                        Itens.Remove(im);
                        item = im;
                        Espaco -= im.Espaco * quantidade;
                        break;
                }
                return true;
            }
            return false;
        }
    }
}
