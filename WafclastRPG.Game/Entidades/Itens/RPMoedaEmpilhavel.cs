using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class RPMoedaEmpilhavel : WafclastItem
    {
        public RPMoedaEmpilhavel(int dropLevel, string tipoBase, RPClasse classe, int espaco, int pilhaMaxima) : base(dropLevel, tipoBase, classe, espaco)
        {
            PilhaAtual = 1;
            PilhaMaxima = pilhaMaxima;
        }

        public int PilhaAtual { get; set; }
        public int PilhaMaxima { get; set; }
    }
}
