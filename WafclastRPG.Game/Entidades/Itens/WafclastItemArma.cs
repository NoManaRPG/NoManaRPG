using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Enums;
using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Game.Entidades.Itens
{
    public class WafclastItemArma : WafclastItem
    {
        public int Nivel { get; private set; }
        public WafclastClasse Classe { get; private set; }
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
        public WafclastDano DanoFisicoBase { get; private set; }
        public double DanoFisicoCriticoChance { get; private set; } = 0;

        public WafclastItemArma(int nivel, string nome, WafclastTipo tipo, WafclastClasse classe,
            int ocupaEspaco, WafclastDano danoFisico, double chanceCritico)
            : base(nome, tipo, ocupaEspaco)
        {
            this.Nivel = nivel;
            this.Classe = classe;
            this.DanoFisicoBase = danoFisico;
            this.DanoFisicoCriticoChance = chanceCritico;
        }
    }
}
