namespace WafclastRPG.Entities
{
    public class WafclastStatePoints
    {
        public decimal CurrentValue { get; set; }
        public decimal MaxValue { get; set; }

        public decimal _baseValue;
        public decimal BaseValue
        {
            get => _baseValue;
            set
            {
                _baseValue = value;
                MaxValue = _baseValue * (_multValue / 100);
            }
        }

        private decimal _multValue;
        /// <summary>
        /// Por padrão é 1.
        /// </summary>
        public decimal MultValue
        {
            get => _multValue;
            set
            {
                _multValue = value;
                MaxValue = BaseValue * (_multValue / 100);
            }
        }

        public WafclastStatePoints(decimal baseValue)
        {
            CurrentValue = baseValue;
            BaseValue = baseValue;
            MultValue = 100;
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
            {
                CurrentValue = 0;
                return true;
            }
            return false;
        }
    }
}
