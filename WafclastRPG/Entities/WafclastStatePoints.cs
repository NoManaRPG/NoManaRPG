using System;

namespace WafclastRPG.Entities {
  public class WafclastStatePoints {
    public double Max { get; set; }
    public double Current { get; set; }


    public WafclastStatePoints(double baseValue) {
      Max = baseValue;
      Current = baseValue;
    }

    public void Restart() => Current = Max;

    public void Add(double value) {
      Current += value;
      if (Current >= Max)
        Current = Max;
    }

    public bool Remove(double value) {
      Current -= value;
      if (Current <= 0)
        return true;
      return false;
    }

    public static bool operator !=(WafclastStatePoints left, double right) {
      if (left.Current != right)
        return true;
      return false;
    }

    public static bool operator ==(WafclastStatePoints left, double right) {
      if (left.Current == 0)
        return true;
      return false;
    }
  }
}