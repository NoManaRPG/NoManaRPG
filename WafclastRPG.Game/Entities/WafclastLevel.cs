using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace WafclastRPG.Game.Entities
{
    public abstract class WafclastLevel
    {
        public int Level { get; set; } = 1;
        public double ExperienciaAtual { get; private set; }
        public double ExperienciaProximoNivel { get; private set; }

        public WafclastLevel() => this.ExperienciaProximoNivel = this.ExperienceTotalLevel(2);

        public WafclastLevel(int startLevel)
        {
            this.Level = startLevel;
            this.ExperienciaProximoNivel = this.ExperienceTotalLevel(startLevel + 1);
        }

        public bool AddExp(double experience)
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
            this.Level++;
            this.ExperienciaProximoNivel = this.ExperienceTotalLevel(Level + 1);
            this.ExperienciaAtual = this.ExperienceTotalLevel(Level);
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
                cm.MapMember(c => c.ExperienciaAtual).SetSerializer(new DoubleSerializer(BsonType.Double, new RepresentationConverter(true, true)));
                cm.MapMember(c => c.ExperienciaProximoNivel).SetSerializer(new DoubleSerializer(BsonType.Double, new RepresentationConverter(true, true)));
            });
        }
    }
}
