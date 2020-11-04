using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(WafclastPontoRegenerativo))]
    public class WafclastPonto
    {
        [BsonIgnore]
        public double Calculado
        {
            get
            {
                var baseCalc = Base * PorcentagemMultiplicador;
                var extraCalc = Extra + baseCalc;
                return extraCalc;
            }
        }

        [BsonElement]
        private double Extra { get; set; } = 0;
        [BsonElement]
        private double Base { get; set; } = 1;
        [BsonElement]
        private double PorcentagemMultiplicador { get; set; } = 1;

        public void WithBase(double valor)
        {
            if (valor <= 0)
                throw new ArgumentOutOfRangeException("valor", "Valor não pode ser negativo ou 0!");
            this.Base = valor;
        }
        public void WithExtra(double valor)
        {
            if (valor < 0)
                throw new ArgumentOutOfRangeException("valor", "Valor não pode ser negativo!");
            this.Extra = valor;
        }
        public void WithMultiplicador(double valor)
        {
            if (valor < 1)
                valor = 1;
            this.PorcentagemMultiplicador = valor;
        }
    }
}
