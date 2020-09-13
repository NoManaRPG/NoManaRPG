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
        public RPItem TryRemoveItem(int index, int quantidade = 1)
        {
            var item = Itens.ElementAtOrDefault(index);
            if (item != null)
            {
                //   3 > 3
                if (quantidade > item.Quantidade)
                    return null;
                item.Quantidade -= quantidade;
                if (item.Quantidade == 0)
                    Itens.RemoveAt(index);
                var itemClone = item.Clone();
                itemClone.Quantidade = quantidade;
                Espaco -= item.Espaco * quantidade;
                return itemClone;
            }
            return null;
        }
    }
}
