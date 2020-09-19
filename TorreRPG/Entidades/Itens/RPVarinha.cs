using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPVarinha : RPArma
    {
        public RPVarinha(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int inteligencia) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Inteligencia = inteligencia;
        }
    }
}
