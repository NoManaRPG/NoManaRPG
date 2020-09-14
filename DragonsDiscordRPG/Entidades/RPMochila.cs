using DragonsDiscordRPG.Extensoes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;
using System.Linq;

namespace DragonsDiscordRPG.Entidades
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
            var itemMochila = Itens.ElementAtOrDefault(index);
            item = null;
            if (itemMochila != null)
            {
                //   3 > 3
                if (quantidade > itemMochila.Quantidade)
                    return false;
                itemMochila.Quantidade -= quantidade;
                if (itemMochila.Quantidade == 0)
                    Itens.RemoveAt(index);

                item = itemMochila.Clone();
                item.Quantidade = quantidade;
                Espaco -= itemMochila.Espaco * quantidade;
                return true;
            }
            return false;
        }
    }
}
