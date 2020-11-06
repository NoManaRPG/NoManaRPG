using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Operations;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastMochila
    {
        public List<WafclastItem> Itens { get; set; } = new List<WafclastItem>();
        public int Moedas { get; private set; }
        public int EspacoAtual { get; private set; }
        public int EspacoMax { get; private set; } = 64;
        public int Nivel { get; set; } = 1;

        public WafclastMochila() { }

        public void AdicionarMoeda(int quantidade)
        {
            Moedas += quantidade;
        }

        public void RemoverMoeda(int quantidade)
        {
            Moedas += quantidade;
        }

        public bool TryAddItem(WafclastItem item, int quantidade = 1)
        {
            if ((item.OcupaEspaco * quantidade) + EspacoAtual > EspacoMax)
                return false;
            EspacoAtual += item.OcupaEspaco * quantidade;
            switch (item)
            {
                case WafclastItemEmpilhavel ie:
                    ie.Pilha += quantidade;
                    var encontrou = Itens.Find(x => x.Nome == ie.Nome);
                    if (encontrou != null)
                        ((WafclastItemEmpilhavel)encontrou).Pilha += quantidade;
                    else
                        Itens.Add(ie);
                    break;
                case WafclastItemArma ia:
                    for (int i = 0; i < quantidade; i++)
                        Itens.Add(ia);
                    break;
            }
            return true;
        }

        public bool TryRemoveItem(int index, out WafclastItem item, int quantidade = 1)
        {
            item = null;
            var mochilaItem = Itens.ElementAtOrDefault(index);
            if (mochilaItem == null)
                return false;

            if (mochilaItem is WafclastItemEmpilhavel)
            {
                if (quantidade > (mochilaItem as WafclastItemEmpilhavel).Pilha)
                    return false;
                var ie = (mochilaItem.Clone() as WafclastItemEmpilhavel);
                ie.Pilha = quantidade;
                var mm = (mochilaItem as WafclastItemEmpilhavel);
                mm.Pilha -= quantidade;
                if (mm.Pilha == 0)
                    Itens.RemoveAt(index);
                EspacoAtual -= ie.OcupaEspaco * ie.Pilha;
                item = ie;
            }
            else
            {
                EspacoAtual -= item.OcupaEspaco;
                Itens.RemoveAt(index);
                item = mochilaItem;
            }
            return true;
        }
    }
}
