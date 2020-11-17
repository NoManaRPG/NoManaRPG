namespace WafclastRPG.Game.Entidades.Itens
{
    public class WafclastItemComida : WafclastItemNormal
    {
        public int Cura { get; set; }

        public WafclastItemComida(int itemId, string nome, double precoCompra, int cura) : base(itemId, nome, precoCompra)
        {
            Cura = cura;
        }
    }
}
