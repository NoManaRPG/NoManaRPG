using MongoDB.Bson.Serialization;
using System;

namespace WafclastRPG.Entities
{
    public class WafclastCoins
    {
        private const ulong SilverInCopper = 100;
        private const ulong GoldInCopper = SilverInCopper * 100;
        private const ulong PlatinumInCopper = GoldInCopper * 100;

        public ulong Coins { get; set; } = 0;

        public WafclastCoins() { }
        public WafclastCoins(ulong startCoins) => Coins = startCoins;

        public void Add(ulong gold, ulong silver, ulong copper)
        {
            Coins += gold * GoldInCopper;
            Coins += silver * SilverInCopper;
            Coins += copper;
        }

        public void Subtract(int coins) => Coins -= Convert.ToUInt64(coins);

        public override string ToString()
        {
            return $"**{GetGold()}** Ouro, **{GetSilver()}** Prata e **{GetCopper()}** Cobre.";
        }

        public ulong GetCopper() => Coins % SilverInCopper;
        public ulong GetSilver() => Coins % GoldInCopper / SilverInCopper;
        public ulong GetGold() => Coins / GoldInCopper;

        #region Static Methods
        public static ulong GetCopper(ulong baseDenomination) => baseDenomination % SilverInCopper;
        public static ulong GetSilver(ulong baseDenomination) => baseDenomination % GoldInCopper / SilverInCopper;
        public static ulong GetGold(ulong baseDenomination) => baseDenomination / GoldInCopper;
        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastCoins>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }
        #endregion
    }
}
