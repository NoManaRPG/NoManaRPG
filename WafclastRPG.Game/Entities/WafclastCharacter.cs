using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacter : WafclastLevel
    {
        public WafclastCoins Coins { get; private set; }

        public WafclastCharacter()
        {
        }

        public void AddLevel()
        {
            Level++;
        }
        public void RemoveLevel()
        {
            Level--;
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastCharacter>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}
