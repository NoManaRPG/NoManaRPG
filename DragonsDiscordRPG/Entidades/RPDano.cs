using MongoDB.Bson.Serialization.Attributes;

namespace DragonsDiscordRPG.Entidades
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
    }
}
