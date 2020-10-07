using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPBaseCurrency : RPBaseItem
    {
        public RPBaseCurrency(int dropLevel, string tipoBase, RPClasse classe, int espaco, int pilhaMaxima) : base(dropLevel, tipoBase, classe, espaco)
        {
            PilhaAtual = 0;
            PilhaMaxima = pilhaMaxima;
        }

        public int PilhaAtual { get; set; }
        public int PilhaMaxima { get; set; }
    }
}
