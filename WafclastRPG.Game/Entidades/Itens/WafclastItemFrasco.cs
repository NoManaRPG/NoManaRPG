using System;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class WafclastItemFrasco : WafclastItemEmpilhavel
    {
        public double VidaRestaura { get; set; }
        public double ManaRestaura { get; set; }
        public FrascoTipo Tipo { get; set; }

        public WafclastItemFrasco(string nome, int ocupaEspaco, double precoCompra,
            double vidaRestaura, double manaRestaura, FrascoTipo tipo)
            : base(nome, ocupaEspaco, precoCompra)
        {
            this.VidaRestaura = vidaRestaura;
            this.ManaRestaura = manaRestaura;
            this.Tipo = tipo;
        }
    }

    [Flags]
    public enum FrascoTipo
    {
        Vida = 1 << 0,
        Mana = 1 << 1,
    }
}
