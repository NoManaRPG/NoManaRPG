using MongoDB.Bson.Serialization;
using System;

namespace WafclastRPG.Game.Entities
{
    public class WafclastMonster
    {
        public ulong Id { get; private set; }
        public ulong MonsterId { get; private set; }
        public string Nome { get; private set; }
        public decimal Defesa { get; private set; }
        public decimal Ataque { get; private set; }
        public decimal VidaAtual { get; private set; }
        public decimal VidaMaxima { get; private set; }
        public decimal Exp { get; private set; }

        public DateTime DateSpawn { get; private set; } = DateTime.UtcNow;
        public TimeSpan SpawnTime { get; private set; } = TimeSpan.FromMinutes(1);

        public WafclastMonster(ulong id, ulong monsterId, string nome, decimal defesa, decimal ataque, decimal vidaMaxima, decimal exp, TimeSpan spawnTime)
        {
            Id = id + monsterId;
            MonsterId = monsterId;
            Nome = nome;
            Defesa = defesa;
            Ataque = ataque;
            VidaMaxima = vidaMaxima;
            VidaAtual = vidaMaxima;
            Exp = exp;
            SpawnTime = spawnTime;
        }

        /// <summary>
        /// Retorna true caso tenha sido abatido.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool ReceberDano(decimal valor)
        {
            VidaAtual -= valor;
            if (VidaAtual <= 0)
            {
                DateSpawn = DateTime.UtcNow + SpawnTime;
                VidaAtual = VidaMaxima;
                return true;
            }
            return false;
        }

        public void SetVida(decimal valor) => VidaAtual = valor;

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
