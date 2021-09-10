using System;
using WafclastRPG.DataBases;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Entities.Characters {
  public class WafclastWarrior : WafclastBaseCharacter {
    public override string EmojiAttack { get; set; } = Emojis.Adaga;

    public override double CalculateDamagePoints() {
      return CalculatePhysicalDamage(Attributes);
    }

    public override void ResetCombatThings() {

    }
  }
}
