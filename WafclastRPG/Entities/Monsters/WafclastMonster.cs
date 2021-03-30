using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;

namespace WafclastRPG.Entities.Monsters
{
    public class WafclastMonster
    {
        /// <summary>
        /// ChannelId + MonsterId
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// MonsterId... #1, #2 ....
        /// </summary>
        public ulong MonsterId { get; set; }
        public string Nome { get; set; }

        public WafclastStatePoints Life { get; set; } = new WafclastStatePoints();
        public decimal MaxAttack { get; set; }
        public decimal Exp { get; set; }

        public DateTime DateSpawn { get; private set; } = DateTime.UtcNow;

        public WafclastMonsterAtributos Atributos { get; set; }

        public TimeSpan RespawnTime { get; set; } = TimeSpan.FromMinutes(1);

        public List<ItemChance> Drops { get; set; } = new List<ItemChance>();

        public WafclastMonster(ulong id, ulong monsterId)
        {
            Id = $"{id}:{monsterId}";
            MonsterId = monsterId;
        }

        public void CalcAtributos()
        {
            Random rd = new Random();
            Atributos.Forca = rd.Next(Atributos.ForcaMin, Atributos.ForcaMin + 1);
            Atributos.Resistencia = rd.Next(Atributos.ResistenciaMin, Atributos.ResistenciaMax + 1);
            Atributos.Agilidade = rd.Next(Atributos.AgilidadeMin, Atributos.AgilidadeMax + 1);

            MaxAttack = Atributos.Forca * 3;
            Life = new WafclastStatePoints(Atributos.Resistencia * 8);
            Exp = (decimal)rd.NextDouble() * (Atributos.ExpMax - Atributos.ExpMin) + Atributos.ExpMin;
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
                CalcAtributos();
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
