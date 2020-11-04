using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class RPBaseItemEquipavel : WafclastItem
    {
        // Requer
        public int Inteligencia { get; set; }
        public int Destreza { get; set; }
        public int Forca { get; set; }

        public RPBaseItemEquipavel(int dropLevel, string tipoBase, RPClasse classe, int espaco) : base(dropLevel, tipoBase, classe, espaco)
        {
        }
    }
}
