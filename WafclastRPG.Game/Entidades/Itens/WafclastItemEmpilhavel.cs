using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastItemEmpilhavel : WafclastItem
    {
        public WafclastItemEmpilhavel(string nome, int ocupaEspaco, double precoCompra)
            : base(nome, ocupaEspaco, precoCompra)
        {
            this.Pilha = 1;
        }

        public int Pilha { get; set; }

    }
}
