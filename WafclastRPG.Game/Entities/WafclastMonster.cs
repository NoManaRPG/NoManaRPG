using System;

namespace WafclastRPG.Game.Entities
{
    public class WafclastMonster
    {
        public ulong Id { get; private set; }
        public ulong MonsterId { get; private set; }
        public string Nome { get; private set; }
        public decimal Defesa { get; private set; } = 0;
        public decimal Ataque { get; private set; } = 0;
        public decimal Vida { get; private set; } = 0;

        public DateTime DateSpawn { get; private set; }

        public WafclastMonster(ulong id, ulong monsterId)
        {
            Id = id + monsterId;
            MonsterId = monsterId;
        }
    }
}
