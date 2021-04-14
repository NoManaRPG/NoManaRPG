namespace WafclastRPG.Entities
{
    public class WafclastStatePoints
    {
        public double CurrentValue { get; set; }
        public double MaxValue { get; set; }

        public double _baseValue;
        public double BaseValue
        {
            get => _baseValue;
            set
            {
                _baseValue = value;
                MaxValue = _baseValue * (_multValue / 100);
            }
        }

        private double _multValue;
        /// <summary>
        /// Por padrão é 1.
        /// </summary>
        public double MultValue
        {
            get => _multValue;
            set
            {
                _multValue = value;
                MaxValue = BaseValue * (_multValue / 100);
            }
        }

        public WafclastStatePoints(double baseValue)
        {
            CurrentValue = baseValue;
            BaseValue = baseValue;
            MultValue = 100;
        }

        public void Restart() => CurrentValue = MaxValue;

        public void Add(double value)
        {
            CurrentValue += value;
            if (CurrentValue >= MaxValue)
                CurrentValue = MaxValue;
        }

        public bool Remove(double value)
        {
            CurrentValue -= value;
            if (CurrentValue <= 0)
            {
                CurrentValue = 0;
                return true;
            }
            return false;
        }
    }
}
