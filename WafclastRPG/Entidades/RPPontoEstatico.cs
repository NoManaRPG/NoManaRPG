using MongoDB.Bson.Serialization.Attributes;

namespace WafclastRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPPontoEstatico
    {
        public double Modificado { get; set; }
        public double Base { get; set; }
        public double Extra { get; set; }
        public double PorcentagemAdicional { get; set; }

        public RPPontoEstatico()
        {
            PorcentagemAdicional = 1;
        }

        public RPPontoEstatico(double atual) : this()
        {
            Modificado = atual;
            Base = atual;
        }
    }
}
