using MongoDB.Bson.Serialization.Attributes;
using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    public class RPArmaArco : RPBaseItemArma
    {
        public RPArmaArco(int dropLevel, string tipoBase, RPClasse classe, int espaco, RPDano danoFisico,
            double chanceCritico, double velocidadeAtaque, int destreza) :
            base(dropLevel, tipoBase, classe, espaco, danoFisico, chanceCritico, velocidadeAtaque)
        {
            Destreza = destreza;
        }

        public override string Descricao()
        {
            return "Arcos\n" + base.Descricao();
        }
    }
}
