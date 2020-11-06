using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastItemComida : WafclastItemEmpilhavel
    {
        public double FomeRestaura { get; set; }

        public WafclastItemComida(string nome, int ocupaEspaco, double precoCompra,
            double fomeRestaura) : base(nome, ocupaEspaco, precoCompra)
        {
            this.FomeRestaura = fomeRestaura;
        }
    }
}