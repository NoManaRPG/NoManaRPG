using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Globalization;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;

namespace WafclastRPG.Entities
{
    public class WafclastPlayer
    {
        public DatabaseSession banco;

        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastCharacter Character { get; private set; }

        public ulong MonsterKill { get; set; }
        public ulong PlayerKill { get; set; }
        public ulong Deaths { get; set; }


        public WafclastPlayer(ulong id)
        {
            this.Id = id;
            this.DateAccountCreation = DateTime.UtcNow;
            this.Character = new WafclastCharacter();
        }

        public static void MapBuilder()
        {
            BsonClassMap.RegisterClassMap<WafclastPlayer>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Id);
                cm.UnmapMember(C => C.banco);
            });
        }

        public Task SaveAsync() => this.banco.ReplacePlayerAsync(this);
        public string Mention()
            => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>";

        /// <summary>
        /// Não salva o usuario.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task ItemAdd(WafclastBaseItem item, int quantity)
        {
            item.PlayerId = Id;
            if (!item.CanStack)
            {
                item.Quantity = 1;
                for (int i = 0; i < quantity; i++)
                {
                    await this.banco.InsertItemAsync(item);
                    Character.Inventory.Quantity++;
                    Character.Inventory.QuantityDifferentItens++;
                }
                return;
            }

            var itemFound = await this.banco.FindItemByItemIdAsync(item.ItemID, Id);
            if (itemFound == null)
            {
                item.Quantity = quantity;
                await this.banco.InsertItemAsync(item);
                Character.Inventory.Quantity += quantity;
                Character.Inventory.QuantityDifferentItens++;
                return;
            }

            itemFound.Quantity += quantity;
            await this.banco.ReplaceItemAsync(itemFound);
            Character.Inventory.Quantity += quantity;
        }
    }
}
