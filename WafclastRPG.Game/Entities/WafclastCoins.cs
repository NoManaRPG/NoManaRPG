using MongoDB.Bson.Serialization;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCoins
    {
        private const ulong SilverInCopper = 100;
        private const ulong GoldInCopper = SilverInCopper * 100;
        private const ulong PlatinumInCopper = GoldInCopper * 100;

        private ulong Coins { get; set; }

        public WafclastCoins() => Coins = 0;
        public WafclastCoins(ulong startCoins) => Coins = startCoins;

        public void Add(ulong gold, ulong silver, ulong copper)
        {
            Coins += gold * GoldInCopper;
            Coins += silver * SilverInCopper;
            Coins += copper;
        }

        public bool Subtract(ulong gold, ulong silver, ulong copper)
        {
            ulong total = 0;
            total += gold * GoldInCopper;
            total += silver * SilverInCopper;
            total += copper;

            if (Coins >= total)
            {
                Coins -= total;
                return true;
            }
            return false;
        }

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
        public static ulong GetGold(ulong coins) => coins / GoldInCopper;
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
