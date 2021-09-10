using System;
using System.Collections.Generic;
using System.Text;
using WafclastRPG.DataBases;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Entities.Characters {
  public class WafclastMage : WafclastBaseCharacter {

    public override string EmojiAttack { get; set; } = Emojis.Dardo;
    public override WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

    public WafclastMage() {
      Skills.Add("Slash", 1);
    }

    public override double CalculateDamagePoints() {
      return CalculateMagicalDamage(Attributes);
    }

    public override void ResetCombatThings() {

    }
  }
}
