using System;
using System.Collections.Generic;
using System.Text;
using TorreRPG.Entidades.Itens.Currency;
using TorreRPG.Enuns;

namespace TorreRPG.Metadata.Itens.Currency
{
    public class CurrencyPergaminho
    {
        public List<RPCurrencyPergaminho> CurrencyPergaminhoAb()
        {
            var pergamin = new List<RPCurrencyPergaminho>();
            pergamin.Add(Pergaminho1());
            return pergamin;
        }

        public RPCurrencyPergaminho Pergaminho1()
        => new RPCurrencyPergaminho(0, "Pergaminho", RPClasse.Pergaminho, 1, 40);
        public RPCurrencyPergaminho PergaminhoFragmento1()
        => new RPCurrencyPergaminho(0, "Fragmento de Pergaminho", RPClasse.FragmentoPergaminho, 1, 20);
    }
}
