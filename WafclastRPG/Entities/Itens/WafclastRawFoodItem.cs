using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastRawFoodItem : WafclastBaseItem
    {
        /// <summary>
        /// Determina o nível minimo para poder cozinhar o item.
        /// </summary>
        public int CookingLevel { get; set; }

        /// <summary>
        /// Determina a chance de sucesso ao cozinhar o item. A chance aumenta com o nível.
        /// </summary>
        public double Chance { get; set; }

        /// <summary>
        /// Quantos de experiencia ganha se cozinhar o item.
        /// </summary>
        public double ExperienceGain { get; set; }

        /// <summary>
        /// Id do item para se transformar caso este item tenha sido cozinhado com sucesso.
        /// </summary>
        public ObjectId CookedItemId { get; set; }

        public WafclastRawFoodItem(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
