using WafclastRPG.Enuns;

namespace WafclastRPG.Entidades.Itens
{
    public class RPArmaVarinha : RPBaseItemArma
    {
        public RPArmaVarinha(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int inteligencia) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Inteligencia = inteligencia;
        }
    }
}
