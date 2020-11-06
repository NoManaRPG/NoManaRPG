using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Game.Metadata.Itens
{
    public sealed class Frascos
    {
        public static WafclastItemFrasco FrascoVidaPequenoAb()
          => new WafclastItemFrasco("Frasco de vida pequeno", 4, 15, 70, 0, FrascoTipo.Vida);
    }
}
