using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.MercadoGeral
{
    [BsonIgnoreExtraElements]
    public class Acao
    {
        /// <summary>
        /// Nome do item.
        /// </summary>
        [BsonId]
        public string Id { get; set; }

        /// <summary>
        /// Soma de os valores negociados.
        /// </summary>
        public ulong ValoresNegociados { get; set; }

        /// <summary>
        /// Quantidade de pessoas que negociou.
        /// </summary>
        public ulong QuantidadeNegociado { get; set; }

        /// <summary>
        /// Preço medio por um item.
        /// </summary>
        public ulong PrecoMedio { get; set; }
    }
}
