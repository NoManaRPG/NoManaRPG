using TorreRPG.Entidades.Itens;
using TorreRPG.Extensoes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using TorreRPG.Enuns;
using TorreRPG.Metadata.Itens.MoedasEmpilhaveis;
using System.Collections;

namespace TorreRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPMochila
    {
        public List<RPBaseItem> Itens { get; set; }

        public int Espaco { get; set; } // Max 64

        public RPMochila()
        {
            Itens = new List<RPBaseItem>();
        }

        public bool TryAddItem(RPBaseItem item, int quantidade = 1)
        {
            switch (item)
            {
                case RPMoedaEmpilhavel me:
                    switch (me.Classe)
                    {
                        case RPClasse.FragmentoPergaminho:
                            // Procura na mochila outros fragmentos
                            int index = Itens.FindIndex(x => x.Classe == RPClasse.FragmentoPergaminho);

                            // Não achou
                            if (index == -1)
                                return AdicionarNovoItem(item, quantidade);

                            // Achou? converte
                            var fragmentos = (RPMoedaEmpilhavel)Itens[index];
                            // Adiciona a pilha
                            fragmentos.PilhaAtual += quantidade;

                            // Se tiver 5 fragmentos
                            if (fragmentos.PilhaAtual >= fragmentos.PilhaMaxima)
                            {
                                // Transformar os fragmentos em pergaminho
                                Itens.RemoveAt(index);
                                Espaco -= 1;

                                return AdicionarCurrency(item, quantidade);
                            }
                            return true;
                        case RPClasse.PergaminhoSabedoria:
                            return AdicionarCurrency(item, quantidade);
                        case RPClasse.PergaminhoPortal:
                            // Procura na mochila outros pergaminhos
                            var pergaminhoPortal = Itens.FindAll(x => x.Classe == RPClasse.PergaminhoPortal);
                            // Não achou
                            if (pergaminhoPortal == null)
                                return AdicionarNovoItem(item, quantidade);
                            // Verifica se o pergaminho está com pilha máxima.
                            foreach (var perg in pergaminhoPortal)
                            {
                                if ((perg as RPMoedaEmpilhavel).PilhaAtual == 40)
                                    continue;
                                else
                                {
                                    (perg as RPMoedaEmpilhavel).PilhaAtual++;
                                    return true;
                                }
                            }
                            return AdicionarNovoItem(item, quantidade);
                    }
                    break;
            }
            return AdicionarNovoItem(item);
        }

        /// <summary>
        /// Retorna null caso não tenha a quantidade especificada.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        public bool TryRemoveItem(int index, out RPBaseItem outItem, int quantidade = 1)
        {
            outItem = null;
            var item = Itens.ElementAtOrDefault(index);

            if (item != null)
            {
                switch (item)
                {
                    case RPMoedaEmpilhavel me:
                        if (me.PilhaAtual < quantidade)
                            return false;
                        me.PilhaAtual -= quantidade;
                        if (me.PilhaAtual == 0)
                        {
                            Itens.Remove(item);
                            Espaco -= item.Espaco;
                        }
                        outItem = item;
                        break;
                    default:
                        switch (item.Classe)
                        {
                            default:
                                Itens.Remove(item);
                                outItem = item;
                                Espaco -= item.Espaco * quantidade;
                                break;
                        }
                        break;
                }
                return true;
            }
            return false;
        }

        public bool TryRemoveItemCurrency(RPClasse classe, out RPBaseItem outItem, int quantidade = 1)
        {
            outItem = null;
            var todosCurrency = Itens.FindAll(x => x.Classe == classe);
            int quantidadeTotal = 0;

            // Soma todas as currency igual
            foreach (var item in todosCurrency)
                quantidadeTotal += (item as RPMoedaEmpilhavel).PilhaAtual;

            if (todosCurrency.Count != 0)
            {
                if (quantidadeTotal < quantidade)
                    return false;
                var item = (todosCurrency[0] as RPMoedaEmpilhavel).Clone();
                item.PilhaAtual = quantidade;
                outItem = item;

                // Subtrai de todas as currency
                while (quantidade != 0)
                {
                    for (int i = todosCurrency.Count - 1; i >= 0; i--)
                    {
                        if (quantidade != 0)
                        {
                            var itemConvertido = todosCurrency[i] as RPMoedaEmpilhavel;
                            itemConvertido.PilhaAtual--;
                            quantidade--;
                            if (itemConvertido.PilhaAtual == 0)
                            {
                                Itens.Remove(itemConvertido);
                                todosCurrency.RemoveAt(i);
                                Espaco -= itemConvertido.Espaco;
                            }
                            if (quantidade == 0)
                                return true;
                        }
                    }
                    if (todosCurrency.Count == 0)
                        break;
                }
            }
            return false;
        }

        private bool AdicionarNovoItem(RPBaseItem item, int quantidade = 1)
        {
            if ((Espaco + item.Espaco) <= 64)
            {
                if (item is RPMoedaEmpilhavel)
                    (item as RPMoedaEmpilhavel).PilhaAtual = quantidade;
                Itens.Add(item);
                Espaco += item.Espaco;
                return true;
            }
            return false;
        }

        private bool AdicionarCurrency(RPBaseItem item, int quantidade)
        {
            // Procura na mochila outros pergaminhos
            var currency = Itens.FindAll(x => x.Classe == item.Classe);
            // Não achou
            if (currency == null)
                return AdicionarNovoItem(item, quantidade);
            // Verifica se o currency está com pilha máxima.
            foreach (var perg in currency)
            {
                if (quantidade != 0)
                {
                    var moedaConvertida = perg as RPMoedaEmpilhavel;
                    if (moedaConvertida.PilhaAtual == moedaConvertida.PilhaMaxima)
                        continue;
                    else
                    {
                        while (quantidade != 0)
                        {
                            moedaConvertida.PilhaAtual++;
                            quantidade--;
                            if (moedaConvertida.PilhaAtual == moedaConvertida.PilhaMaxima)
                                return AdicionarNovoItem(item, quantidade);
                            if (quantidade == 0)
                                return true;
                        }
                    }
                }
            }
            return AdicionarNovoItem(item, quantidade);
        }
    }
}
