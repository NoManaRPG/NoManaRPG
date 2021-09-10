﻿using System;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;

namespace WafclastRPG {
  public static class Mathematics {
    public static double CalculateLifePoints(WafclastAttributes attributes) => (attributes.Constitution * 8.0) + (attributes.Strength / 5.0) + ((attributes.Constitution / 5.0) * 3.0);
    public static double CalculateManaPoints(WafclastAttributes attributes) => (attributes.Intelligence * 3.0) + (attributes.Perception / 3.0) + (attributes.Constitution / 3.0);

    public static double CalculatePhysicalDamage(WafclastAttributes attributes) => attributes.Strength + ((attributes.Strength / 5.0) * 2.0) + (attributes.Dexterity / 2.0);
    public static double CalculateMagicalDamage(WafclastAttributes attributes) => attributes.Intelligence + ((attributes.Intelligence / 5.0) * 2.0) + (attributes.Dexterity / 2.0);

    public static double CalculateEvasionPoints(WafclastAttributes attributes) => (attributes.Agility * 2.0) + (attributes.Dexterity / 5.0) + (attributes.Intelligence / 5.0) + (attributes.Perception / 3.0) + (attributes.Willpower / 3.0);
    public static double CalculateDexteryPoints(WafclastAttributes attributes) => (attributes.Dexterity * 2.0) + (attributes.Agility / 5.0) + (attributes.Intelligence / 5.0) + (attributes.Perception / 3.0) + (attributes.Willpower / 3.0);

    public static double CalculateAttackSpeed(WafclastAttributes attributes) => (attributes.Dexterity / 4);

    public static bool CalculateHitChance(double attackerDexteryPoints, double defenderEvasionPoints) {
      var chance = (attackerDexteryPoints / defenderEvasionPoints) * 0.75;
      var rd = new Random();
      return rd.Chance(chance);
    }
  }
}