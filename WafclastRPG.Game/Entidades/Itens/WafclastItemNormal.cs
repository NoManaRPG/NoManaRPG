namespace WafclastRPG.Game.Entidades.Itens
{
    /// <summary>
    /// Para itens que não podem se juntar.
    /// </summary>
    public class WafclastItemNormal : WafclastItem
    {
        public WafclastItemNormal(int itemId, string nome, double precoCompra) : base(itemId, nome, precoCompra)
        {
        }
    }
}
