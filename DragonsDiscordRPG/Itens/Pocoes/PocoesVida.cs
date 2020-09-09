using DragonsDiscordRPG.Entidades;

namespace DragonsDiscordRPG.Itens.Pocoes
{
    public static class PocoesVida
    {
        public static RPItem Frasco()
        {
            var item = new RPItem(Enuns.RPTipo.PocaoVida, "Pequeno frasco de vida", 1, "https://static.wikia.nocookie.net/pathofexile_gamepedia/images/4/43/Small_Life_Flask_inventory_icon.png/revision/latest/scale-to-width-down/58?cb=20191001051037")
            {
                CargasAtual = 0,
                CargasUso = 7,
                CargasMax = 21,
                LifeRegen = 70,
                Tempo = 6,
            };
            return item;
        }
    }
}
