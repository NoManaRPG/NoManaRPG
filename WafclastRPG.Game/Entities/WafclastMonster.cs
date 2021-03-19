using MongoDB.Bson.Serialization;
using System;

namespace WafclastRPG.Game.Entities
{
    public class WafclastMonster
    {
        /// <summary>
        /// ChannelId + MonsterId
        /// </summary>
        public ulong Id { get; private set; }

        /// <summary>
        /// MonsterId... #1, #2 ....
        /// </summary>
        public ulong MonsterId { get; private set; }
        public string Nome { get; private set; }

        public decimal MaxAttack { get; private set; }
        public decimal Exp { get; private set; }

        public WafclastStatePoints Life { get; set; } = new WafclastStatePoints();
        public WafclastMonsterAtributos Atributo { get; set; }

        public DateTime DateSpawn { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Tempo de respawn após morto.
        /// </summary>
        public TimeSpan RespawnTime { get; set; } = TimeSpan.FromMinutes(1);

        public WafclastMonster(ulong id, ulong monsterId, string nome, decimal exp)
        {
            Id = id;
            MonsterId = monsterId;
            Nome = nome;
            Exp = exp;
        }

        public void CalcAtributos()
        {
            MaxAttack = Atributo.Forca * 3;
            Life = new WafclastStatePoints(Atributo.Resistencia * 8);
        }

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
                DateSpawn = DateTime.UtcNow + RespawnTime;
                Life.Restart();
                return true;
            }
            return false;
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
