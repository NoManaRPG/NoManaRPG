using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace WafclastRPG.Entities
{
    public abstract class WafclastLevel
    {
        public int Level { get; set; } = 1;
        public decimal ExperienciaAtual { get; set; }
        public decimal ExperienciaProximoNivel { get; private set; }
        public int LevelBloqueado { get; set; } = 0;

        public WafclastLevel() => this.ExperienciaProximoNivel = this.ExperienceTotalLevel(2);

        public WafclastLevel(int startLevel)
        {
            this.Level = startLevel;
            this.ExperienciaProximoNivel = this.ExperienceTotalLevel(startLevel + 1);
        }

        public int ReceberExperiencia(decimal experience)
        {
            int niveisEv = 0;
            decimal expResultante = this.ExperienciaAtual + experience;
            if (expResultante >= this.ExperienciaProximoNivel)
            {
                do
                {
                    expResultante = expResultante - this.ExperienciaProximoNivel;
                    this.Evoluir();
                    niveisEv++;
                } while (expResultante >= this.ExperienciaProximoNivel);
                this.ExperienciaAtual += expResultante;
                return niveisEv;
            }
            this.ExperienciaAtual += experience;
            return niveisEv;
        }

        public void DiminuirLevel()
        {
            ExperienciaAtual = 0;
            if (Level == 1)
                return;

            if (Level > LevelBloqueado)
                LevelBloqueado = Level;

            Level--;
            ExperienciaProximoNivel = this.ExperienceTotalLevel(Level + 1);
            this.ExperienciaAtual = this.ExperienceTotalLevel(Level);
        }

        private void Evoluir()
        {
            this.Level++;
            this.ExperienciaProximoNivel = this.ExperienceTotalLevel(Level + 1);
        }

        // Needs to be 83 for level 2.
        private int ExperienceTotalLevel(int level)
        {
            double v1 = 1.0 / 8.0 * level * (level - 1.0) + 75.0;
            double pow1 = Math.Pow(2, (level - 1.0) / 7.0) - 1;
            double pow2 = 1 - Math.Pow(2, -1 / 7.0);
            return (int)Math.Truncate(v1 * (pow1 / pow2));
        }

        public static void MapBuilderLevel()
        {
            BsonClassMap.RegisterClassMap<WafclastLevel>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapMember(c => c.ExperienciaAtual).SetSerializer(new DecimalSerializer(BsonType.Decimal128, new RepresentationConverter(true, true)));
                cm.MapMember(c => c.ExperienciaProximoNivel).SetSerializer(new DecimalSerializer(BsonType.Decimal128, new RepresentationConverter(true, true)));
            });
        }
    }
}
