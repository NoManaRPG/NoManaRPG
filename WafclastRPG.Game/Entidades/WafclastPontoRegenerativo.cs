using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastPontoRegenerativo : WafclastPonto
    {
        public double Atual { get; private set; }
        [BsonIgnore]
        public double Maximo { get => Calculado; }
        public double RegenPorSegundo { get; private set; }

        public void Diminuir(double valor)
        {
            Atual -= valor;
            if (Atual < 0)
                Atual = 0;
        }

        public void Incrementar(double valor)
        {
            Atual += valor;
            if (Atual > Calculado)
                Atual = Calculado;
        }

        public void WithRegen(double valor)
        {
            if (valor < 0)
                throw new ArgumentOutOfRangeException("valor", "Valor não pode ser negativo!");
            this.RegenPorSegundo = valor;
        }
    }
}
