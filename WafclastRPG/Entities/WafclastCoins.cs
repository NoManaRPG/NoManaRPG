namespace WafclastRPG.Entities
{
    public class WafclastCoins
    {
        public ulong Coins { get; set; } = 0;

        public WafclastCoins(ulong startCoins) => Coins = startCoins;

        public void Add(ulong quantidade)
            => Coins += quantidade;

        public void Subtract(ulong quantidade)
            => Coins -= quantidade;

        public override string ToString()
        {
            return Coins.ToString();
        }
    }
}
