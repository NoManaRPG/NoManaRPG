using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Game.Enums;

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
        public bool IsDuasMao { get; private set; }

        public WafclastItemArma(int id, string nome, int ocupaEspaco,
            WafclastClasse classe, WafclastDano danoFisico, double chanceCritico, bool duasMao = false)
            : base(id, nome, ocupaEspaco)
        {
            this.Classe = classe;
            this.DanoFisicoBase = danoFisico;
            this.DanoFisicoCriticoChance = chanceCritico;
            this.IsDuasMao = duasMao;
        }
    }
}
