using static WafclastRPG.Mathematics;

namespace WafclastRPG.Entities.Wafclast {
  public class CharacterMage : BaseCharacter {

    public override string EmojiAttack { get; set; } = Emojis.Dardo;
    public override WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

    public CharacterMage() {
      Skills.Add("Slash", 1);
    }

    public override double CalculateDamagePoints() {
      return CalculateMagicalDamage(Attributes);
    }

    public override void ResetCombatThings() {

    }
  }
}
