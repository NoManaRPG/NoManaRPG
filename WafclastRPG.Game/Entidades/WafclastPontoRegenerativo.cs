using MongoDB.Bson.Serialization.Attributes;
using System;

namespace WafclastRPG.Game.Entidades
{
    [BsonIgnoreExtraElements]
    public class WafclastPontoRegenerativo : WafclastPonto
    {
        [BsonIgnore]
        public double Maximo { get => Calculado; }
        public double Atual { get; private set; }
        public double RegenPorSegundo { get; private set; }

        public void Diminuir(double valor)
        {
            if (valor > 0)
                throw new ArgumentOutOfRangeException("valor", "Valor não pode ser positivo!");
            Atual -= valor;
            if (Atual < 0)
                Atual = 0;
        }

        public void Incrementar(double valor)
        {
            if (valor < 0)
                throw new ArgumentOutOfRangeException("valor", "Valor não pode ser negativo!");
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
