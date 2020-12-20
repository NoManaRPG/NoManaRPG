namespace WafclastRPG.Game.Entidades.NPC
{
    public class WafclastMonstroDrop
    {
        public int ItemId { get; set; }
        public Raridade Chance { get; set; }
        /// <summary>
        /// Is actual quantity too
        /// </summary>
        public int QuantidadeMin { get; set; }
        public int QuantidadeMax { get; set; }

        public WafclastMonstroDrop(Raridade chance, int itemId, int quantidadeMax)
        {
            this.Chance = chance;
            this.ItemId = itemId;
            this.QuantidadeMin = 1;
            this.QuantidadeMax = quantidadeMax;
        }

        public WafclastMonstroDrop(Raridade chance, int itemId, int quantidadeMin, int quantidadeMax)
        {
            this.Chance = chance;
            this.ItemId = itemId;
            this.QuantidadeMin = quantidadeMin;
            this.QuantidadeMax = quantidadeMax;
        }

        public class Raridade
        {
            public int ValorMenor { get; set; }
            public int ValorMaior { get; set; }
            public double Chance { get { return (double)ValorMenor / (double)ValorMaior; } }

            public Raridade(int valorMenor, int valorMaior)
            {
                this.ValorMenor = valorMenor;
                this.ValorMaior = valorMaior;
            }
        }
    }
}
