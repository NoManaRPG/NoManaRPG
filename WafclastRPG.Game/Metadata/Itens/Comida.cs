using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Metadata.Itens
{
    public class Comida
    {
        public static WafclastItem CarneCruAb()
            => new WafclastItemEmpilhavel(1, "Carne cru", 2);
    }
}
