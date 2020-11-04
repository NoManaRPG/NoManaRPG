using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class RPArmaCetro : RPBaseItemArma
    {
        public RPArmaCetro(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int forca, int inteligencia) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Forca = forca;
            Inteligencia = inteligencia;
        }
    }
}
