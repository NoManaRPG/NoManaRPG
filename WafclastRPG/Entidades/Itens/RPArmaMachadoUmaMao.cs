using WafclastRPG.Enuns;

namespace WafclastRPG.Entidades.Itens
{
    public class RPArmaMachadoUmaMao : RPBaseItemArma
    {
        public RPArmaMachadoUmaMao(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int forca, int destreza) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Forca = forca;
            Destreza = destreza;
        }
    }
}
