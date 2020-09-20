using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPAdaga : RPArma
    {
        public RPAdaga(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int destreza, int inteligencia) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Destreza = destreza;
            Inteligencia = inteligencia;
        }
    }
}
