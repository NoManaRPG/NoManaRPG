using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class RPBaseItemArma : RPBaseItemEquipavel
    {
        public RPDano DanoFisicoBase { get; set; }
        public RPDano DanoFisicoModificado { get; set; }
        public double ChanceCritico { get; set; }
        public double VelocidadeAtaque { get; set; }

        public RPBaseItemArma(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque) : base(dropLevel, tipoBase, classe, espaco)
        {
            DanoFisicoBase = danoFisico;
            DanoFisicoModificado = danoFisico;
            ChanceCritico = chanceCritico;
            VelocidadeAtaque = velocidadeAtaque;
        }
    }
}
