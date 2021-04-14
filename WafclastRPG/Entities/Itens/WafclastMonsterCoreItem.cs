using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastMonsterCoreItem : WafclastBaseItem
    {
        public double ExperienceGain { get; set; }

        public WafclastMonsterCoreItem(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
