using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(WafclastItemArma), typeof(WafclastItemEmpilhavel))]
    public class WafclastItem
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public int OcupaEspaco { get; private set; }

        public WafclastItem(int id, string nome, int ocupaEspaco)
        {
            this.Id = id;
            this.Nome = nome;
            this.OcupaEspaco = ocupaEspaco;
        }
    }
}
