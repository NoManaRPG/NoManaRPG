using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Entities.Wafclast {

  public enum ActionType {
    Stun,
    Damage,
  }

  public class BaseSkill {
    //Mana, SP etc
    public double ResourceCost { get; set; }

    public List<ActionType>

  }
}
