using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCharacter
    {
        public int Level { get; private set; }
        public WafclastCoins Coins { get; private set; }

        public WafclastCharacter()
        {
            Level = 0;
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
