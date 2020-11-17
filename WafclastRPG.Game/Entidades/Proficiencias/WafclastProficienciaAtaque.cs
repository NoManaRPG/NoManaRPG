using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel;

namespace WafclastRPG.Game.Entidades.Proficiencias
{
    /// <summary>
    /// Controla a precisão e os tipos de armas que podem ser equipadas
    /// </summary>
    [BsonIgnoreExtraElements]
    [Description("Teste")]
    public class WafclastProficienciaAtaque : WafclastProficiencia
    {
        public override string Descricao
        {
            get
            {
                return "Ataque é uma habilidade de combate corpo a corpo" +
" que determina a precisão dos ataques corpo a corpo de um jogador e os" +
" tipos de armas que um jogador pode empunhar. Um nível de ataque mais alto " +
"significa que os jogadores podem usar armas melhores, que possuem maior dano e precisão.";
            }
        }
        public int Precisao { get { return PrecisaoBase + PrecisaoExtra; } }
        public int PrecisaoBase { get; set; }
        public int PrecisaoExtra { get; set; }

        public WafclastProficienciaAtaque()
        {
            this.PrecisaoBase = (int)Math.Truncate(this.CalcularPrecisao());
        }

        public double CalcularPrecisao()
            => (0.0008 * Math.Pow(this.Nivel, 3)) + (4 * this.Nivel) + 40;

        public double ChanceAcerto(double precisao, double defesa)
            => 0.55 * (precisao / defesa);
    }
}
