using WafclastRPG.Enuns;

namespace WafclastRPG.Entidades.Itens
{
    public class RPMoedaEmpilhavel : RPBaseItem
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
