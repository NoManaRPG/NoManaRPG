// This file is part of the WafclastRPG project.

namespace WafclastRPG.Game.Entities
{
    public class WafclastStatePoints
    {
        public double Max { get; set; }
        public double Current { get; set; }

        public WafclastStatePoints(double baseValue)
        {
            this.Max = baseValue;
            this.Current = baseValue;
        }

        public void Restart() => this.Current = this.Max;

        public void Increment(double value)
        {
            this.Current += value;
            if (this.Current >= this.Max)
                this.Current = this.Max;
        }
        public bool Decrement(double value)
        {
            this.Current -= value;
            if (this.Current <= 0)
                return true;
            return false;
        }
    }
}
