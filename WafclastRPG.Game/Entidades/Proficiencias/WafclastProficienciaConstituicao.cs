using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Game.Entidades.Proficiencias
{
    [BsonIgnoreExtraElements]
    public class WafclastProficienciaConstituicao : WafclastProficiencia
    {
        public override string Descricao
        {
            get
            {
                return "Constituição (também conhecida como saúde, Hitpoints, HP ou Hits) é uma" +
                    " habilidade de combate que afeta quantos pontos de vida (LP)" +
                    " um jogador ou monstro possui. Os pontos de vida representam " +
                    "a quantidade de dano que um jogador ou monstro pode suportar antes " +
                    "de morrer. A morte ocorre quando os pontos de vida de um jogador ou monstro" +
                    " chegam a zero.";
            }
        }

        public int Vida { get; set; }

        public WafclastProficienciaConstituicao(int nivel = 10) : base(nivel)
        {
            this.Vida = this.CalcularVida();
        }

        public int CalcularVida()
            => this.Nivel * 100;

        public bool RemoveVida(int valor)
        {
            this.Vida -= valor;
            if (this.Vida <= 0)
                return true;
            return false;
        }

        public void AddVida(int valor)
        {
            this.Vida += valor;
            var vidaTotal = CalcularVida();
            if (this.Vida >= vidaTotal)
                this.Vida = vidaTotal;
        }
    }
}
