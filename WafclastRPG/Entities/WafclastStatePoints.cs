using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastStatePoints
    {
        public double BaseValue { get; set; }
        public double CurrentValue { get; set; }

        public WafclastStatePoints(double baseValue)
        {
            BaseValue = baseValue;
            CurrentValue = baseValue;
        }

        public void Restart() => CurrentValue = BaseValue;

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
        public void Add(double value)
        {
            CurrentValue += value;
            if (CurrentValue >= BaseValue)
                CurrentValue = BaseValue;
        }
    }
}