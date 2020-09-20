using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPMacaUmaMao : RPArma
    {
        public RPMacaUmaMao(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int forca) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Forca = forca;
        }
    }
}
