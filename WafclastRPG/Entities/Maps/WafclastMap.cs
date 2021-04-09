using MongoDB.Bson.Serialization;
using System.Collections.Generic;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Enums;

namespace WafclastRPG.Entities.Maps
{
    public class WafclastMap
    {
        /// <summary>
        /// Text Channel Id
        /// </summary>
        public ulong Id { get; set; }

        /// <summary>
        /// Discord Guild Id
        /// </summary>
        public ulong GuildId { get; set; }
        public MapType Tipo { get; set; }
        public WafclastCoordinates Coordinates { get; set; }
        public int QuantidadeMonstros { get; set; } = 0;

        public List<WafclastBaseItem> ShopItens { get; set; } = new List<WafclastBaseItem>();

        public WafclastMap(ulong textChannelId, ulong guildId)
        {
            Id = textChannelId;
            GuildId = guildId;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastMap>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
            });
        }
    }
}
