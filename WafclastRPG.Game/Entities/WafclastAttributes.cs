using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace WafclastRPG.Game.Entities {

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

    public IEnumerable<(WafclastAttribute Attribute, string Name)> GetAttributes() {
      foreach (PropertyInfo propertyInfo in GetType().GetProperties()) {
        yield return ((WafclastAttribute) propertyInfo.GetValue(this, null), TranslateAttribute(propertyInfo.Name));
      }
    }

    private string TranslateAttribute(string name)
#pragma warning disable CS8509 // A expressão switch não manipula todos os valores possíveis de seu tipo de entrada (não é exaustiva).
      => name switch {
#pragma warning restore CS8509 // A expressão switch não manipula todos os valores possíveis de seu tipo de entrada (não é exaustiva).
        "Strength" => "Força",
        "Constitution" => "Constituição",
        "Dexterity" => "Destreza",
        "Agility" => "Agilidade",
        "Intelligence" => "Inteligencia",
        "Willpower" => "Força de Vontade",
        "Perception" => "Percepção",
        "Charisma" => "Carisma",
      };
  }


  [BsonIgnoreExtraElements]
  public class WafclastAttribute {
    public double Base { get; set; }
    public double BonusPositive { get; set; }
    public double BonusNegative { get; set; }
    public double Current { get { return (Base + BonusPositive) - BonusNegative; } }

    public WafclastAttribute(double baseValue) {
      Base = baseValue;
    }

    public void IncrementBase(double value) => Base += value;

    public static double operator *(WafclastAttribute attribute, int value) => attribute.Current * Convert.ToDouble(value);
    public static double operator *(WafclastAttribute attribute, double value) => attribute.Current * value;
    public static double operator +(WafclastAttribute attribute, double value) => attribute.Current + value;
    public static double operator +(double value, WafclastAttribute attribute) => value + attribute.Current;
    public static double operator /(WafclastAttribute attribute, double value) => attribute.Current / value;
    public static double operator /(double value, WafclastAttribute attribute) => attribute.Current / value;

    public override string ToString() {
      return Current.ToString();
    }
  }
}