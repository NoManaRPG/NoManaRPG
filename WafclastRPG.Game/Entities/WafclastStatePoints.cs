namespace WafclastRPG.Game.Entities
{
    public class WafclastStatePoints
    {
        public decimal CurrentValue { get; set; }
        public decimal MaxValue { get; set; }

        public WafclastStatePoints(decimal minValue, decimal maxValue)
        {
            CurrentValue = minValue;
            MaxValue = maxValue;
        }

        public WafclastStatePoints(decimal maxValue)
        {
            MaxValue = maxValue;
            CurrentValue = maxValue;
        }

        public WafclastStatePoints()
        {
        }

        public void Restart() => CurrentValue = MaxValue;

        public void Add(decimal value)
        {
            CurrentValue += value;
            if (CurrentValue >= MaxValue)
                CurrentValue = MaxValue;
        }

        public bool Remove(decimal value)
        {
            CurrentValue -= value;
            if (CurrentValue <= 0)
                return true;
            return false;
        }
    }
}
