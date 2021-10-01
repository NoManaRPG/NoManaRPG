using static WafclastRPG.Mathematics;

namespace WafclastRPG.Entities.Wafclast {
  public class MageCharacter : BaseCharacter {

    public override string EmojiAttack { get; set; } = Emojis.Dardo;
    public override WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

    public MageCharacter() {
    }
  }

  public string test() {
      return   "sad";
  }


  public override double Calcul  ate  DamagePoints() {
    return CalculateMagicalDamage(Attributes);
  }

  public override void ResetCombatThings() {

  }
}
}
