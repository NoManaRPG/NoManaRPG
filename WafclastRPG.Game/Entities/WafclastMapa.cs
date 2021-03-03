using MongoDB.Bson.Serialization;

namespace WafclastRPG.Game.Entities
{
    public class WafclastMapa
    {
        public ulong Id { get; private set; }
        public WafclastMapaType Tipo { get; private set; }
        public int QuantidadeMonstros { get; private set; } = 0;
        public WafclastMapa(ulong id, WafclastMapaType tipo)
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
