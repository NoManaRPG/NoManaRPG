using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades.Proficiencias
{
    [BsonIgnoreExtraElements]
    public class WafclastProficienciaDefesa : WafclastProficiencia
    {
        public override string Descricao
        {
            get
            {
                return "A defesa é uma habilidade de combate que garante proteção aos" +
                    " jogadores em todas as formas de combate.";
            }
        }

        public int Defesa { get { return DefesaBase + DefesaExtra; } }
        public int DefesaBase { get; set; }
        public int DefesaExtra { get; set; }

        public WafclastProficienciaDefesa()
        {
            this.DefesaBase = (int)Math.Truncate(this.CalcularDefesa());
        }

        public double CalcularDefesa()
            => (0.0008 * Math.Pow(this.Nivel, 3)) + (4 * this.Nivel) + 40;
    }
}
