using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastAttribute
    {
        public double Base { get; set; }
        public double BonusPositive { get; set; }
        public double BonusNegative { get; set; }
        public double Current { get { return (Base + BonusPositive) - BonusNegative; } }

        public WafclastAttribute(double baseValue)
        {
            Base = baseValue;
        }

        public static double operator *(WafclastAttribute left, int right) => left.Current * Convert.ToDouble(right);
        public static double operator *(WafclastAttribute left, double right) => left.Current * right;
        public static double operator +(WafclastAttribute left, double right) => left.Current + right;
        public static double operator +(double right, WafclastAttribute left) => right + left.Current;
        public static double operator /(WafclastAttribute left, double right) => left.Current / right;
    }
}