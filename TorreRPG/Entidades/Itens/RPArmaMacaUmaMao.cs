using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPArmaMacaUmaMao : RPBaseItemArma
    {
        public RPArmaMacaUmaMao(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int forca) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Forca = forca;
        }
    }
}
