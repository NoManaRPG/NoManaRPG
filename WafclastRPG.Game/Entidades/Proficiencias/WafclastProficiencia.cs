using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades.Proficiencias
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(WafclastProficienciaAtaque), typeof(WafclastProficienciaConstituicao),
                    typeof(WafclastProficienciaDefesa), typeof(WafclastProficienciaForca))]
    public abstract class WafclastProficiencia
    {
        public int Nivel { get; set; }
        public bool IsElite { get; set; } = false;

        public virtual string Descricao { get; }

        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int ExperienciaAtual { get; private set; }
        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int ExperienciaProximoNivel { get; private set; }

        public WafclastProficiencia()
        {
            this.Nivel = 1;
            this.ExperienciaProximoNivel = this.ExperienceTotalLevel(2);
        }

        public WafclastProficiencia(int startLevel)
        {
            this.Nivel = startLevel;
            this.ExperienciaProximoNivel = this.ExperienceTotalLevel(startLevel + 1);
        }

        public bool AddExperience(double experience)
        {
            double expResultante = this.ExperienciaAtual + experience;
            if (expResultante >= this.ExperienciaProximoNivel)
            {
                do
                {
                    double quantosPrecisaProxNivel = expResultante - this.ExperienciaProximoNivel;
                    this.Evoluir();
                    expResultante = quantosPrecisaProxNivel;
                } while (expResultante >= this.ExperienciaProximoNivel);
                this.ExperienciaAtual += (int)Math.Truncate(expResultante);
                return true;
            }
            this.ExperienciaAtual += (int)Math.Truncate(experience);
            return false;
        }

        private void Evoluir()
        {
            this.Nivel++;
            this.ExperienciaProximoNivel = this.ExperienceTotalLevel(Nivel + 1);
            this.ExperienciaAtual = this.ExperienceTotalLevel(Nivel);
        }

        // Needs to be 83 for level 2.
        private int ExperienceTotalLevel(int level)
        {
            double v1 = 1.0 / 8.0 * level * (level - 1.0) + 75.0;
            double pow1 = Math.Pow(2, (level - 1.0) / 7.0) - 1;
            double pow2 = 1 - Math.Pow(2, -1 / 7.0);
            return (int)Math.Truncate(v1 * (pow1 / pow2));
        }
    }
}
