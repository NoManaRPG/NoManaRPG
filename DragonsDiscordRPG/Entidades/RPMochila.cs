using DragonsDiscordRPG.Extensoes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPMochila
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public SortedDictionary<string, RPItem> Itens { get; set; }

        public int Espaco { get; set; } // Max 64

        //public string AdicionarItem(RPGItem item, int quantidade = 1) => AdicionarItem(item.Nome, quantidade);

        public bool AddItem(RPItem item, int quantidade = 1)
        {
            if (Espaco + (item.Espaco * quantidade) <= 64)
            {
                int index = 1;
                while (true)
                {
                    // Adicionar na pilha fazer codigo
                    string nome = $"{item.Tipo.ToString().ToLower()}:{index}";
                    if (Itens.TryAdd(nome, item)) return true;
                    index++;
                }
            }
            return false;
        }

        /// <summary>
        /// Retorna null caso não tenha a quantidade especificada.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        public RPItem RemoveItem(string nome, int quantidade = 1)
        {
            nome = nome.ToLower();
            if (Itens.TryGetValue(nome, out RPItem item))
            {
                //   3 > 3
                if (quantidade > item.Quantidade)
                    return null;
                item.Quantidade -= quantidade;
                if (item.Quantidade == 0)
                    Itens.Remove(nome);
                var itemClone = item.Clone();
                itemClone.Quantidade = quantidade;
                return itemClone;
            }
            return null;
        }
    }
}
