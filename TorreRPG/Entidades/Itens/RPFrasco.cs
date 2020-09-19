using MongoDB.Bson.Serialization.Attributes;
using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    public class RPFrasco : RPBaseItem
    {
        public RPFrasco(int dropLevel, string tipoBase, RPClasse classe, int espaco, double regen,
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

        public void Resetar()
            => CargasAtual = CargasMax;
    }
}
