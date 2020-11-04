using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class RPBaseFrasco : WafclastItem
    {
        public RPBaseFrasco(int dropLevel, string tipoBase, RPClasse classe, int espaco, double regen,
            double tempo, double cargasUso, double cargasMax) :
            base(dropLevel, tipoBase, classe, espaco)
        {
            Regen = regen;
            Tempo = tempo;
            CargasUso = cargasUso;
            CargasMax = cargasMax;
        }

        public double Regen { get; set; }
        public double Tempo { get; set; }
        public double CargasUso { get; set; }
        public double CargasMax { get; set; }
        public double CargasAtual { get; set; }

        public void AddCarga(double valor)
        {
            CargasAtual += valor;
            if (CargasAtual > CargasMax) CargasAtual = CargasMax;
        }

        public bool RemoverCarga(double valor)
        {
            if (CargasAtual >= valor)
            {
                CargasAtual -= valor;
                return true;
            }
            return false;
        }

        public void ResetarCargas()
            => CargasAtual = CargasMax;
    }
}
