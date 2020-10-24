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

        public bool TryAddItem(RPBaseItem item)
        {
            if (item is RPMoedaEmpilhavel)
                return AdicionarMoeda(item as RPMoedaEmpilhavel);
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
                if (item is RPMoedaEmpilhavel)
                    return TryRemoveItemCurrency((item as RPMoedaEmpilhavel).Classe, out outItem, quantidade);
                Itens.Remove(item);
                outItem = item;
                Espaco -= item.Espaco;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Usar somente para moedas.
        /// </summary>
        /// <param name="classe"></param>
        /// <param name="outItem"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
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

        private bool AdicionarNovoItem(RPBaseItem item)
        {
            if ((Espaco + item.Espaco) <= 64)
            {
                Itens.Add(item);
                Espaco += item.Espaco;
                return true;
            }
            return false;
        }

        private bool AdicionarMoeda(RPMoedaEmpilhavel item)
        {
            // Procura na mochila outros iguais
            var todasMoedas = Itens.FindAll(x => x.Classe == item.Classe);

            if (todasMoedas == null)
            {
                // Não achou? Adiciona o item com 1 de pilha
                item.PilhaAtual = 1;
                return AdicionarNovoItem(item);
            }

            // Verifica cada moeda
            foreach (var baseItem in todasMoedas)
            {
                var moeda = baseItem as RPMoedaEmpilhavel;

                if ((moeda.PilhaAtual + 1) == moeda.PilhaMaxima)
                {
                    switch (moeda.Classe)
                    {
                        case RPClasse.FragmentoPergaminho:
                            Itens.Remove(moeda);
                            Espaco -= item.Espaco;
                            return AdicionarMoeda(new MoedasEmpilhaveis().PergaminhoSabedoria());
                    }
                    moeda.PilhaAtual++;
                    return true;
                }
                else if (moeda.PilhaAtual < moeda.PilhaMaxima)
                {
                    moeda.PilhaAtual++;
                    return true;
                }
            }
            return AdicionarNovoItem(item);
        }
    }
}
