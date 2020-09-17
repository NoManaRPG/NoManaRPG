using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    public class RPArma : RPItem
    {
        public RPDano DanoFisico { get; set; }
        public double ChanceCritico { get; set; }
        public double VelocidadeAtaque { get; set; }

        public RPArma(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque) : base(dropLevel, tipoBase, classe, espaco)
        {
            DanoFisico = danoFisico;
            ChanceCritico = chanceCritico;
            VelocidadeAtaque = velocidadeAtaque;
        }

        public virtual string Descricao()
        {
            return $"Dano Físico: {DanoFisico.Minimo}-{DanoFisico.Maximo}\n" +
                $"Chance de Crítico: {ChanceCritico * 100}%\n" +
                $"Ataques por Segundo: {VelocidadeAtaque}\n" +
                $"---------------\n" +
                $"Requer Nível {ILevel}, {(Inteligencia == 0 ? "" : $"{Inteligencia} Int,")} {(Destreza == 0 ? "" : $"{Destreza} Des,")} {(Forca == 0 ? "" : $"{Forca} For")}";
        }
    }
}
