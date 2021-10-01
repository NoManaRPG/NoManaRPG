using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Entities.Wafclast {
  public class MageCharacter : BaseCharacter {

    public override string EmojiAttack { get; set; } = Emojis.Dardo;
    public override WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

    public MageCharacter() {
    }
    public override double CalculateDamagePoints() {
      return CalculateMagicalDamage(Attributes);
    }

    public override void ResetCombatThings() {

    }
  }
}
