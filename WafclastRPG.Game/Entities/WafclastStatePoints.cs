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

        public void Add(double value)
        {
            this.Current += value;
            if (this.Current >= this.Max)
                this.Current = this.Max;
        }
        public bool Remove(double value)
        {
            this.Current -= value;
            if (this.Current <= 0)
                return true;
            return false;
        }

        #region Operators
        public static bool operator !=(WafclastStatePoints left, double right) => !(left == right);
        public static bool operator ==(WafclastStatePoints left, double right)
        {
            if (left.Current == right)
                return true;
            return false;
        }
        #endregion
    }
}