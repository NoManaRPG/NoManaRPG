using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens.Currency
{
    public class RPCurrencyPergaminho : RPBaseCurrency
    {
        public RPCurrencyPergaminho(int dropLevel, string tipoBase, RPClasse classe, int espaco, int pilhaMaxima) : base(dropLevel, tipoBase, classe, espaco, pilhaMaxima)
        {
        }

        public void Identificar(RPBaseItem item)
        {

        }
        public virtual string Descricao()
        {
            return $"Permite identificar um item mágico, raro ou único.";
        }
    }
}
