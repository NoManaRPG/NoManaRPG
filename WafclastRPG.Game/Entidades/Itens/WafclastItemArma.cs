using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Game.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    public class WafclastItemArma : WafclastItem
    {
        public int Nivel { get; set; }
        public WafclastClasse Classe { get; set; }
        [BsonIgnore]
        public WafclastDano DanoFisicoCalculado
        {
            get
            {
                var dano = new WafclastDano(DanoFisicoBase);
                int nivel = this.Nivel / 5;
                dano.Maximo *= nivel;
                dano.Minimo *= nivel;
                return dano;
            }
        }
        public WafclastDano DanoFisicoBase { get; set; }
        public double DanoFisicoCriticoChance { get; set; } = 0;
        public bool IsDuasMao { get; set; }

        public WafclastItemArma(string nome, int ocupaEspaco, double precoCompra,
            WafclastClasse classe, WafclastDano danoFisico, double chanceCritico,
            bool duasMao = false) : base(nome, ocupaEspaco, precoCompra)
        {
            this.Classe = classe;
            this.DanoFisicoBase = danoFisico;
            this.DanoFisicoCriticoChance = chanceCritico;
            this.IsDuasMao = duasMao;
        }
    }
}
