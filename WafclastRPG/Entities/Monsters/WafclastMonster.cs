using System;
using System.Collections.Generic;

namespace WafclastRPG.Entities.Monsters
{
    public class WafclastMonster : WafclastMonsterBase
    {
        public string Name { get; set; }

        public WafclastStatePoints PhysicalDamage { get; set; }
        public WafclastStatePoints Evasion { get; set; }
        public WafclastStatePoints Accuracy { get; set; }
        public WafclastStatePoints Armour { get; set; }
        public WafclastStatePoints Life { get; set; }

        public DateTime DateSpawn { get; private set; } = DateTime.UtcNow;
        public TimeSpan RespawnTime { get; set; } = TimeSpan.FromMinutes(1);

        public bool ItsPillaged { get; set; } = true;
        public List<ItemChance> ChanceDrops { get; set; } = new List<ItemChance>();

        public WafclastMonster(ulong channelId, int monsterId) : base(channelId, monsterId) { }

        /// <summary>
        /// Retorna true caso tenha sido abatido.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool ReceberDano(decimal valor)
        {
            Life.CurrentValue -= valor;
            if (Life.CurrentValue <= 0)
            {
                ItsPillaged = false;
                return true;
            }
            return false;
        }

        public void Restart()
        {
            DateSpawn = DateTime.UtcNow + RespawnTime;

            PhysicalDamage.Restart();
            Evasion.Restart();
            Accuracy.Restart();
            Life.Restart();

            ItsPillaged = true;
        }

        public decimal DamageReduction(decimal damage)
        {
            var first = Armour.CurrentValue * damage;
            var second = (Armour.CurrentValue + 10) * damage;
            var dr = first / second;
            return damage - damage * dr;
        }
    }
}
