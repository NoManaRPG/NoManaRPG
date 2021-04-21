using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastPickaxeItem : WafclastBaseItem
    {
        /// <summary>
        /// Determina qual minério consegue minerar. Se a dureza da picareta for inferior ao do mínerio, não conseguirá minerar.
        /// </summary>
        public double Hardness { get; set; }

        /// <summary>
        /// Bônus extra que faz aumentar a chance de cair o minério.
        /// </summary>
        public double DropChanceBonus { get; set; }

        /// <summary>
        /// Força necessária para empunhar a picareta.
        /// </summary>
        public double Strength { get; set; }

        public WafclastPickaxeItem(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
