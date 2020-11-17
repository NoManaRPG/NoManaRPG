using WafclastRPG.Game.Entidades.Itens;
using static WafclastRPG.Game.Enums.EquipamentoType;

namespace WafclastRPG.Game.Metadata.Itens
{
    public sealed class Armas
    {
        public static WafclastItemArma BronzeDaggerAb()
        {
            var arma = new WafclastItemArma(2, "Adaga de Bronze", 168, 1, 4, PrimeiraMao)
            {
                Examinar = "Pequeno mas pontudo"
            };
            return arma;
        }
    }
}
