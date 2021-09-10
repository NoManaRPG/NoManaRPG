using DSharpPlus;
using System;
using WafclastRPG.Entities;

namespace WafclastRPG.DataBases {
  public class DatabaseRegions {
    public WafclastRegion Region0() {
      var reg = new WafclastRegion(0, "Celeiro") {
        Description = $"Tem algumas vacas aqui."
      };

      var monster = new WafclastMonster(1, "Vaca");
      //var drop1 = new DropChance(0, .9, 1, 2);
      //monster.Drops.Add(drop1);

      reg.Monsters = monster;
      return reg;
    }
  }
}
