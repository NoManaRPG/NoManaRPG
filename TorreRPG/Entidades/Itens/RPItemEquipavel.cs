using MongoDB.Bson.Serialization.Attributes;
using TorreRPG.Enuns;

namespace TorreRPG.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    public class RPItemEquipavel : RPItem
    {
        // Requer
        public int Inteligencia { get; set; }
        public int Destreza { get; set; }
        public int Forca { get; set; }

        public RPItemEquipavel(int dropLevel, string tipoBase, RPClasse classe, int espaco) : base(dropLevel, tipoBase, classe, espaco)
        {
        }
    }
}
