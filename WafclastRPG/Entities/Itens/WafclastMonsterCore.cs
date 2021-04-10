namespace WafclastRPG.Entities.Itens
{
    public class WafclastMonsterCore : WafclastBaseItem
    {
        public decimal ExperienceGain { get; set; }

        public WafclastMonsterCore(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
