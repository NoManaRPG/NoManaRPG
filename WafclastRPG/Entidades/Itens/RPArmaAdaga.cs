using WafclastRPG.Enuns;

namespace WafclastRPG.Entidades.Itens
{
    public class RPArmaAdaga : RPBaseItemArma
    {
        public RPArmaAdaga(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int destreza, int inteligencia) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Destreza = destreza;
            Inteligencia = inteligencia;
        }
    }
}
