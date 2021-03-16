using MongoDB.Bson.Serialization;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Entities
{
    public class WafclastMapa
    {
        /// <summary>
        /// Text Channel Id
        /// </summary>
        public ulong Id { get; private set; }
        public ulong ServerID { get; private set; }
        public MapType Tipo { get; private set; }
        public WafclastCoordinates Coordinates { get; set; }
        public int QuantidadeMonstros { get; set; } = 0;

        public WafclastMapa(ulong id, MapType tipo)
        {
            Id = id;
            Tipo = tipo;
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
