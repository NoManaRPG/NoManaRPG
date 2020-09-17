using TorreRPG.Enuns;
using MongoDB.Bson.Serialization.Attributes;

namespace TorreRPG.Entidades.Itens
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(RPFrasco), typeof(RPArma), typeof(RPArco), typeof(RPFrascoVida), typeof(RPItemEquipavel))]
    public class RPItem
    {
        public int DropLevel { get; set; } // Nível que começa a cair
        public int ILevel { get; set; } // Zona de onde caiu, tudo baseado nisso
        public string TipoBase { private get; set; } // Nome base
        public string TipoBaseModificado { get; set; } // Nome base + prefixos
        public RPClasse Classe { get; set; }
        public RPRaridade Raridade { get; set; }

        // Ocupa
        public int Espaco { get; set; }

        public RPItem(int dropLevel, string tipoBase, RPClasse classe, int espaco)
        {
            DropLevel = dropLevel;
            TipoBase = tipoBase;
            TipoBaseModificado = tipoBase;
            Classe = classe;
            Espaco = espaco;
            Raridade = RPRaridade.Normal;
        }
    }
}
