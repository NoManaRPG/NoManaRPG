// This file is part of NoManaRPG project.

namespace NoManaRPG.Entidades.Monsters;

public class MonsterItemDrop
{
    public int GlobalItemId { get; set; }
    public double Chance { get; set; }

    public MonsterItemDrop(int globalItemId, double chance)
    {
        this.GlobalItemId = globalItemId;
        this.Chance = chance;
    }
}
