using MongoDB.Bson.Serialization.Attributes;

namespace TorreRPG.Entidades
{
    [BsonIgnoreExtraElements]
    public class RPAtributo
    {
        public int Inteligencia { get; private set; }
        public int Destreza { get; private set; }
        public int Forca { get; private set; }

        public RPAtributo(int inteliencia, int destreza, int forca)
        {
            Inteligencia = inteliencia;
            Destreza = destreza;
            Forca = forca;
        }

    }
}
