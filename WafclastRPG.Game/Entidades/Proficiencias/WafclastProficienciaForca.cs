using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades.Proficiencias
{
    /// <summary>
    /// Controla o dano 
    /// </summary>
    [BsonIgnoreExtraElements]
    public class WafclastProficienciaForca : WafclastProficiencia
    {
        public override string Descricao
        {
            get
            {
                return "Força é uma habilidade de combate corpo a corpo que aumenta a" +
                    " quantidade de dano que um jogador pode infligir em combate" +
                    " corpo a corpo em 2,5 pontos por nível. Além disso, a Força " +
                    "pode ser necessária para empunhar certas armas e armaduras, " +
                    "acessar alguns atalhos de Agilidade e completar várias missões.";
            }
        }

        /// <summary>
        /// Dano máximo
        /// </summary>
        public int Dano { get { return DanoBase + DanoExtra; } }
        public int DanoBase { get; set; }
        public int DanoExtra { get; set; }

        public WafclastProficienciaForca()
        {
            this.DanoBase = (int)Math.Truncate(this.CalcularDano());
        }

        public double CalcularDano()
            => this.Nivel * 2.5;
    }
}
