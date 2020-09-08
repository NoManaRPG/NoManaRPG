using MongoDB.Bson.Serialization.Attributes;

namespace DragonsDiscordRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPPontoEstatico
    {
        public double Atual { get; set; }
        public double PorcentagemAdicional { get; set; }

        public RPPontoEstatico()
        {
            PorcentagemAdicional = 1;
        }

        public RPPontoEstatico(double atual) : this()
        {
            Atual = atual;
        }
    }
}
