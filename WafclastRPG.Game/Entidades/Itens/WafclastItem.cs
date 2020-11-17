using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(WafclastItemArma), typeof(WafclastItemComida),
                    typeof(WafclastItemNormal))]
    public class WafclastItem : ICloneable
    {
        [BsonId]
        public int ItemId { get; set; }
        public string Nome { get; set; }
        public double PrecoCompra { get; set; } // Moedas
        public string Examinar { get; set; }

        [BsonIgnoreIfNull]
        public int ReceitaId { get; set; }

        [BsonIgnore]
        public double PrecoVenda { get { return this.PrecoCompra / 2; } }

        public WafclastItem(int itemId, string nome, double precoCompra)
        {
            this.ItemId = itemId;
            this.Nome = nome;
            this.PrecoCompra = precoCompra;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
