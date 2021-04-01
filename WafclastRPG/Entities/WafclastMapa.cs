using MongoDB.Bson.Serialization;
using WafclastRPG.Enums;

namespace WafclastRPG.Entities
{
    public class WafclastMapa
    {
        /// <summary>
        /// Text Channel Id
        /// </summary>
        public ulong Id { get; set; }
        public ulong ServerID { get; set; }
        public MapType Tipo { get; set; }
        public WafclastCoordinates Coordinates { get; set; }
        public int QuantidadeMonstros { get; set; } = 0;

        public WafclastMapa(ulong id, ulong serverID)
        {
            Id = id;
            ServerID = serverID;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastMapa>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
            });
        }
    }
}
