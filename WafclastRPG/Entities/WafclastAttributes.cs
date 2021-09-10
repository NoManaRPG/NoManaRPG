using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Entities {

  [BsonIgnoreExtraElements]
  public class WafclastAttributes {
    public WafclastAttribute Strength { get; set; }
    public WafclastAttribute Constitution { get; set; }
    public WafclastAttribute Dexterity { get; set; }
    public WafclastAttribute Agility { get; set; }
    public WafclastAttribute Intelligence { get; set; }
    public WafclastAttribute Willpower { get; set; }
    public WafclastAttribute Perception { get; set; }
    public WafclastAttribute Charisma { get; set; }

    public WafclastAttributes(double strength = 5, double constitution = 5, double dexterity = 5, double agility = 5, double intelligence = 5, double willpower = 5, double perception = 5, double charisma = 5) {
      Strength = new WafclastAttribute(strength);
      Constitution = new WafclastAttribute(constitution);
      Dexterity = new WafclastAttribute(dexterity);
      Agility = new WafclastAttribute(agility);
      Intelligence = new WafclastAttribute(intelligence);
      Willpower = new WafclastAttribute(willpower);
      Perception = new WafclastAttribute(perception);
      Charisma = new WafclastAttribute(charisma);
    }
  }


  [BsonIgnoreExtraElements]
  public class WafclastAttribute {
    public double Base { get; set; }
    public double BonusPositive { get; set; } = 0;
    public double BonusNegative { get; set; } = 0;
    public double Current { get { return (Base + BonusPositive) - BonusNegative; } }

    public WafclastAttribute(double baseValue) {
      Base = baseValue;
    }

    public static double operator *(WafclastAttribute attribute, int value) => attribute.Current * Convert.ToDouble(value);
    public static double operator *(WafclastAttribute attribute, double value) => attribute.Current * value;
    public static double operator +(WafclastAttribute attribute, double value) => attribute.Current + value;
    public static double operator +(double value, WafclastAttribute attribute) => value + attribute.Current;
    public static double operator /(WafclastAttribute attribute, double value) => attribute.Current / value;
    public static double operator /(double value, WafclastAttribute attribute) => attribute.Current / value;
  }
}