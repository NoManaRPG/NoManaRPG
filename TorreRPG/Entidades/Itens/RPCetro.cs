using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPCetro : RPArma
    {
        public RPCetro(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int forca, int inteligencia) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Forca = forca;
            Inteligencia = inteligencia;
        }
    }
}
