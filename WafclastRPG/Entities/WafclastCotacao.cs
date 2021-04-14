using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Entities
{
    public class WafclastCotacao
    {
        /// <summary>
        /// Id do item
        /// </summary>
        public ObjectId Id { get; set; }
        public double PriceBase { get; set; }
        public ulong QuantitySold { get; set; }
        public ulong QuantityBuy { get; set; }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastCotacao>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
            });
        }

    }
}
