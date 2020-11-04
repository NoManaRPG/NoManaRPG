using WafclastRPG.Game.Enuns;
using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(WafclastItemArma), typeof(WafclastItemEmpilhavel))]
    public class WafclastItem
    {
        public string Nome { get; private set; }
        public WafclastTipo Tipo { get; private set; }
        public int OcupaEspaco { get; private set; }

        public WafclastItem(string nome, WafclastTipo tipo, int ocupaEspaco)
        {
            this.Nome = nome;
            this.Tipo = tipo;
            this.OcupaEspaco = ocupaEspaco;
        }
    }
}
