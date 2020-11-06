using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(WafclastItemArma), typeof(WafclastItemEmpilhavel),
        typeof(WafclastItemBebida), typeof(WafclastItemComida), typeof(WafclastItemFrasco))]
    public class WafclastItem : ICloneable
    {
        public string Nome { get; set; }
        public int OcupaEspaco { get; set; }
        public double PrecoCompra { get; set; }

        public WafclastItem(string nome, int ocupaEspaco, double precoCompra)
        {
            this.Nome = nome;
            this.OcupaEspaco = ocupaEspaco;
            this.PrecoCompra = precoCompra;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
