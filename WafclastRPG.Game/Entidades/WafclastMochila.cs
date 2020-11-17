using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastMochila
    {
        public int EspacoAtual { get; set; } = 0;
        public int EspacoMax { get; private set; } = 28;

        // Index | ItemId | Quantidade
        public List<Item> Itens { get; set; } = new List<Item>();

        public bool TryAddItem(WafclastItem item, int quantidade = 1)
        {
            switch (item)
            {
                case WafclastItemNormal win:
                    EspacoAtual += quantidade;
                    if (EspacoAtual > EspacoMax)
                        return false;
                    else
                    {
                        var itemigual = Itens.Find(x => x.ItemId == win.ItemId);
                        if (itemigual != null)
                            itemigual.Quantidade += quantidade;
                        else
                            Itens.Add(new Item(win.ItemId, quantidade));
                        return true;
                    }
            }
            return false;
        }

        public bool TryRemoveItem(int index, int quantidade, out Item item)
        {
            item = null;
            var itemM = Itens.ElementAtOrDefault(index);
            if (itemM == null)
                return false;
            if (!itemM.Juntavel)
            {
                if (quantidade <= itemM.Quantidade)
                {
                    EspacoAtual -= quantidade;
                    itemM.Quantidade -= quantidade;
                    item = itemM.Clone();
                    if (itemM.Quantidade == 0)
                        Itens.Remove(itemM);
                    return true;
                }
            }
            else { }
            return false;
        }

        public bool TryGetItem(int index, out Item item)
        {
            item = Itens.ElementAtOrDefault(index);
            if (item == null)
                return false;
            return true;
        }

        public class Item
        {
            public int ItemId { get; private set; }
            public int Quantidade { get; set; }
            public bool Juntavel { get; set; }

            public Item(int itemId, int quantidade, bool juntavel = false)
            {
                this.ItemId = itemId;
                this.Quantidade = quantidade;
                this.Juntavel = juntavel;
            }

            public Item Clone()
            {
                return new Item(this.ItemId, this.Quantidade, this.Juntavel);
            }
        }
    }
}
