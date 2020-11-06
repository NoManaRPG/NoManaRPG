using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Metadata.Itens
{
    public sealed class Comidas
    {
        public static WafclastItemComida CarneCruAb()
           => new WafclastItemComida("Carne cru", 2, 10, 5);
    }
}
