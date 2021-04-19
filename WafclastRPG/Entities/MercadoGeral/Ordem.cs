using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.MercadoGeral
{
    [BsonIgnoreExtraElements]
    public class Ordem
    {
        /// <summary>
        /// Id da ordem
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        /// <summary>
        /// Tipo de ordem.
        /// </summary>
        public OrdemType Tipo { get; set; }

        /// <summary>
        /// Id do jogador.
        /// </summary>
        public ulong PlayerId { get; set; }

        /// <summary>
        /// Nome do item.
        /// </summary>
        public string ItemNome { get; set; }

        /// <summary>
        /// Quantidade que está negociando.
        /// </summary>
        public int Quantidade { get; set; }

        /// <summary>
        /// Preço por cada unidade.
        /// </summary>
        public ulong Preço { get; set; }

        /// <summary>
        /// Se ainda está ativa.
        /// </summary>
        public bool Ativa { get; set; }

        /// <summary>
        /// Quantos ganhou
        /// </summary>
        public ulong GanhouVenda { get; set; }
    }

    public enum OrdemType
    {
        Venda,
        Compra
    }
}
