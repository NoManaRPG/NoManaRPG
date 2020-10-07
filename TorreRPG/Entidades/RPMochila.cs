using TorreRPG.Entidades.Itens;
using TorreRPG.Extensoes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using TorreRPG.Entidades.Itens.Currency;
using TorreRPG.Enuns;

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
                case RPCurrencyPergaminho rcp:
                    if (rcp.Classe == RPClasse.FragmentoPergaminho)
                    {
                        // É fragmento
                        // Procura na mochila outros fragmentos
                        int index = Itens.FindIndex(x => x.Classe == RPClasse.FragmentoPergaminho);
                        // Não achou
                        if (index == -1)
                            return Adicionar(item);

                        // Achou
                        var fragmentos = (RPCurrencyPergaminho)Itens[index];

                        // Adiciona
                        fragmentos.PilhaAtual += quantidade;
                        // Se tiver 20 fragmentos
                        if (fragmentos.PilhaAtual >= fragmentos.PilhaMaxima)
                        {
                            // Transformar os fragmentos em pergaminho
                            Itens.RemoveAt(index);

                            // Procura na mochila outros pergaminhos
                            var pergaminho1 = (RPCurrencyPergaminho)Itens.Find(x => x.Classe == RPClasse.Pergaminho);
                            // Não achou
                            if (pergaminho1 == null)
                                return Adicionar(new Metadata.Itens.Currency.CurrencyPergaminho().Pergaminho1());
                            // Achou
                            // Verifica se o pergaminho está com pilha máxima.
                            // Se tiver não adiciona.
                            if (pergaminho1.PilhaAtual == 40)
                                return false;
                            // Adiciona
                            pergaminho1.PilhaAtual++;
                            return true;
                        }
                        return true;
                    }
                    // Procura na mochila outros pergaminhos
                    var pergaminho2 = (RPCurrencyPergaminho)Itens.Find(x => x.Classe == RPClasse.Pergaminho);
                    // Não achou
                    if (pergaminho2 == null)
                        return Adicionar(item);
                    // Achou
                    // Verifica se o pergaminho está com pilha máxima.
                    // Se tiver não adiciona.
                    if (pergaminho2.PilhaAtual == 40)
                        return false;
                    // Adiciona
                    pergaminho2.PilhaAtual++;
                    return true;
            }

            return Adicionar(item);
        }

        /// <summary>
        /// Retorna null caso não tenha a quantidade especificada.
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        public bool TryRemoveItem(int index, out RPBaseItem item, int quantidade = 1)
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

        public bool Adicionar(RPBaseItem item, int quantidade = 1)
        {
            if (Espaco + (item.Espaco * quantidade) <= 64)
            {
                Itens.Add(item);
                Espaco += item.Espaco * quantidade;
                if (item is RPBaseCurrency)
                    (item as RPBaseCurrency).PilhaAtual++;
                return true;
            }
            return false;
        }
    }
}
