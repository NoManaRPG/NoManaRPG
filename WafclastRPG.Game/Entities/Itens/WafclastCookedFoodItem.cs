// This file is part of the WafclastRPG project.

using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entities.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastCookedFoodItem : WafclastBaseItem
    {
        /// <summary>
        /// Quantos ganha de vida ao comer o item.
        /// </summary>
        public double LifeGain { get; set; }

        public WafclastCookedFoodItem(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
