using static WafclastRPG.Mathematics;

namespace WafclastRPG.Game.Entities.Wafclast {
  public class CharacterWarrior : BaseCharacter {
    public override string EmojiAttack { get; set; } = Emojis.Adaga;
    public override WafclastAttributes Attributes { get; set; } = new WafclastAttributes();

    public override double CalculateDamagePoints() {
      return CalculatePhysicalDamage(Attributes);
    }

    public override void ResetCombatThings() {

    }
  }
}
