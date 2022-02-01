// This file is part of WafclastRPG project.

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

        public void ResetCurrentToMax() => this.Current = this.Max;

        public void AddCurrent(double value)
        {
            this.Current += value;
            if (this.Current >= this.Max)
                this.Current = this.Max;
        }
        public bool RemoveCurrent(double value)
        {
            this.Current -= value;
            if (this.Current <= 0)
                return true;
            return false;
        }

        public void ChangeMaxValue(double value)
        {
            if (value < this.Current)
                this.Current = value;
            this.Max = value;
        }
    }
}
