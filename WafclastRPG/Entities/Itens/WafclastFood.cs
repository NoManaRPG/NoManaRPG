namespace WafclastRPG.Entities.Itens
{
    public class WafclastFood : WafclastBaseItem
    {
        public decimal LifeGain { get; set; }

        public WafclastFood(WafclastBaseItem baseItem) : base(baseItem)
        {
        }
    }
}
