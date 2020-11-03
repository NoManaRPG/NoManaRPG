using WafclastRPG.Enuns;

namespace WafclastRPG.Entidades.Itens
{
    public class RPFrascoVida : RPBaseFrasco
    {
        public RPFrascoVida(int dropLevel, string tipoBase, RPClasse classe, int espaco, double regen,
            double tempo, double cargasUso, double cargasMax) :
            base(dropLevel, tipoBase, classe, espaco, regen, tempo, cargasUso, cargasMax)
        {
        }
    }
}
