using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastPickaxeItem : WafclastLevelItem
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
        /// Aumenta os atributos em % a cada nível extra. Operação feita ao evoluir o item.
        /// </summary>
        public int Bonus { get; set; }

        /// <summary>
        /// Força necessária para empunhar a picareta.
        /// </summary>
        public double Strength { get; set; }

        public WafclastPickaxeItem(WafclastBaseItem baseItem) : base(baseItem) { }

        public new bool AddExperience(double exp)
        {
            int levelUps = base.AddExperience(exp);
            for (int i = 0; i < levelUps; i++)
            {
                var bonus = (Bonus / 100) + 1;
                Hardness *= bonus;
                DropChanceBonus *= bonus;
            }

            if (levelUps >= 1)
                return true;
            return false;
        }
    }
}
