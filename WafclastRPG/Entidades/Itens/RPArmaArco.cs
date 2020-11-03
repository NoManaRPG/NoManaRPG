using WafclastRPG.Enuns;

namespace WafclastRPG.Entidades.Itens
{
    public class RPArmaArco : RPBaseItemArma
    {
        public RPArmaArco(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int destreza) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Destreza = destreza;
        }
    }
}
