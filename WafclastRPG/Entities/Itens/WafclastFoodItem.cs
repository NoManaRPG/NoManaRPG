namespace WafclastRPG.Entities.Itens
{
    public class WafclastFoodItem : WafclastBaseItem
    {
        public double LifeGain { get; set; }

        public WafclastFoodItem(WafclastBaseItem baseItem) : base(baseItem) { }
    }
}
