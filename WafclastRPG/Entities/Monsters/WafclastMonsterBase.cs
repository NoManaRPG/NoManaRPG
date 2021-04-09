using MongoDB.Bson.Serialization;

namespace WafclastRPG.Entities.Monsters
{
    public class WafclastMonsterBase
    {
        /// <summary>
        /// textChannelId:monsterId
        /// </summary>
        public string Id { get; private set; }

        public int MonsterId { get; set; }

        public WafclastMonsterBase(ulong textChannelId, int monsterId)
        {
            Id = $"{textChannelId}:{monsterId}";
            MonsterId = monsterId;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastMonsterBase>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
            });
            BsonClassMap.RegisterClassMap<WafclastMonster>();
        }
    }
}
