using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace WafclastRPG.Entities.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastOreItem : WafclastBaseItem
    {
        /// <summary>
        /// Determina qual picareta consegue minerar. Se a dureza da picareta for inferior ao do mínerio, não conseguirá minerar.
        /// </summary>
        public double Hardness { get; set; }

        /// <summary>
        /// Chance do mínerio cair.
        /// </summary>
        public double DropChance { get; set; }

        /// <summary>
        /// Nível minimo multiplicado por 2 para começar a cair mais de 1 minério. Ex. Se for 2, aumenta a chance de +1 a cada 2 nível.
        /// </summary>
        public double MinLevel { get; set; }

        /// <summary>
        /// Tipos de minérios
        /// </summary>
        public OreType Type { get; set; }

        /// <summary>
        /// Quantos de experiencia que ganha por minerio. Ou seja se multiplica pela quantidade minerada.
        /// </summary>
        public double ExperienceGain { get; set; }

        public WafclastOreItem(WafclastBaseItem baseItem) : base(baseItem) { }

        public enum OreType
        {
            [Description("Argila")]
            Clay,
        }
    }
}
