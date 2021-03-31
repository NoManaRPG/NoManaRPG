using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using WafclastRPG.Enums;

namespace WafclastRPG.Entities.Itens
{
    public class WafclastBaseItem
    {
        /*
         * ID
         * ITEMID = ...
         * PlayerID = ...
         * IsStackable
         * 
         * 
         * Ao entrar item novo no inventario do jogador o que fazer:
         * Adicionad um item com o playerId, se for empilhavel, procurar outro com o mesmo ItemID
         * 
         * Para mostrar todos os items, procura pelo o playerid
         * 
         * Sobre as moedas, acredito que é melhor deixalas separadas
         * */


        public ObjectId Id { get; set; }
        public ulong ItemID { get; set; }
        public ulong PlayerId { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; }
        public int Level { get; set; } = 1;
        public int Price { get; set; } = 0;
        public bool CanSell { get; set; } = true;
        public bool CanStack { get; set; } = true;
        public int Quantity { get; set; } = 1;
        public string ImageURL { get; set; } = "";
        public string Description { get; set; } = "Sem descrição";

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastBaseItem>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
                cm.SetIsRootClass(true);
            });
            //BsonClassMap.RegisterClassMap<WafclastBaseItem>();
        }
    }
}
