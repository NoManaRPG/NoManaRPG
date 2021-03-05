using MongoDB.Bson.Serialization;
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
        public decimal VidaMaxima { get; private set; } = 0;
        public decimal Exp { get; private set; } = 0;

        public DateTime DateSpawn { get; set; }

        public WafclastMonster(ulong id, ulong monsterId)
        {
            Id = id + monsterId;
            MonsterId = monsterId;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastMonster>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
            });
        }
    }
}
