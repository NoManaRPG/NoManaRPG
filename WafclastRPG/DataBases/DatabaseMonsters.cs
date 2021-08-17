using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.DataBases
{
    public class DatabaseMonsters
    {
        public WafclastMonster Vaca1Ab()
        {
            var monster = new WafclastMonster();
            monster.Name = "Vaca";
            monster.LifePoints = 100;
            var drop1 = new DropChance(0, .9, 1, 2);
            monster.Drops.Add(drop1);
            return monster;
        }

        public WafclastMonster Espantalho1Ab()
        {
            var monster = new WafclastMonster()
            {
                Level = 1,
                Name = "Espantalho Mágico",
                LifePoints = 100,
                MaxDamage = 100.8,
                AccuracyTotal = 202,
                ArmorTotal = 110,
                AttackRate = 2.4,
            };

            return monster;
        }
    }
}
