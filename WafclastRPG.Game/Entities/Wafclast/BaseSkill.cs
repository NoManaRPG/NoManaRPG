// This file is part of the WafclastRPG project.

namespace WafclastRPG.Game.Entities.Wafclast
{

    public enum ActionType
    {
        Stun,
        Damage,
    }

    public class BaseSkill
    {
        //Mana, SP etc
        public double ResourceCost { get; set; }


    }
}
