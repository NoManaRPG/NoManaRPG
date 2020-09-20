using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPEspada : RPArma
    {
        public RPEspada(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int forca, int destreza) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Forca = forca;
            Destreza = destreza;
        }
    }
}
