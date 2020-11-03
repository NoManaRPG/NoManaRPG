using MongoDB.Bson.Serialization.Attributes;
using WafclastRPG.Services;

namespace WafclastRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPDano
    {
        public double Minimo { get; set; }
        public double Maximo { get; set; }
        public double Sortear { get { return Calculo.SortearValor(Minimo, Maximo); } }

        public RPDano(double minimo, double maximo)
        {
            Minimo = minimo;
            Maximo = maximo;
        }

        public void Somar(RPDano dano)
        {
            Minimo += dano.Minimo;
            Maximo += dano.Maximo;
        }

        public void Subtrair(RPDano dano)
        {
            Minimo -= dano.Minimo;
            Maximo -= dano.Maximo;
        }
    }
}
