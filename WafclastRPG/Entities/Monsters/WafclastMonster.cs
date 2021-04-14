using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace WafclastRPG.Entities.Monsters
{
    public class WafclastMonster
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Name { get; set; }
        public int FloorLevel { get; set; } = 1;

        public WafclastStatePoints PhysicalDamage { get; set; }
        public WafclastStatePoints Evasion { get; set; }
        public WafclastStatePoints Accuracy { get; set; }
        public WafclastStatePoints Armour { get; set; }
        public WafclastStatePoints Life { get; set; }

        public List<DropChance> DropChances { get; set; } = new List<DropChance>();

        /// <summary>
        /// Retorna true caso tenha sido abatido.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool ReceberDano(double valor)
        {
            Life.CurrentValue -= valor;
            if (Life.CurrentValue <= 0)
                return true;
            return false;
        }

        public double DamageReduction(double damage)
        {
            var first = Armour.CurrentValue * damage;
            var second = (Armour.CurrentValue + 10) * damage;
            var dr = first / second;
            return damage - damage * dr;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastMonster>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            });
        }
    }
}
