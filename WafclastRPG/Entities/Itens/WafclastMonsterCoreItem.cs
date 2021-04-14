namespace WafclastRPG.Entities.Itens
{
    public class WafclastMonsterCoreItem : WafclastBaseItem
    {
        public double ExperienceGain { get; set; }

        public WafclastMonsterCoreItem(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
