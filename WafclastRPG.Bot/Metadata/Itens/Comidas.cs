using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Metadata.Itens
{
    public sealed class Comidas
    {
        public static WafclastItemComida CarneCozidaAb()
        {
            var comida = new WafclastItemComida(1, "Carne cozida", 30, 200) { Examinar = "Mmm, isso parece gostoso." };
            return comida;
        }
    }
}
