using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Globalization;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastPlayer
    {
        [BsonId]
        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastCharacter Character { get; private set; }

        public ulong MonsterKill { get; set; }
        public ulong PlayerKill { get; set; }
        public ulong Deaths { get; set; }

        public string Language { get; set; } = "pt-BR";
        public bool Reminder { get; set; }

        public WafclastPlayer(ulong id)
        {
            Id = id;
            DateAccountCreation = DateTime.UtcNow;
            Character = new WafclastCharacter();
        }

        public string Mention { get => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>"; }

        [BsonIgnore]
        public DatabaseSession Session { get; set; }

        /// <summary>
        /// Somente usar em uma sessão.
        /// <para>Retorna um monstro aleatorio baseado no andar do personagem.</para>
        /// </summary>
        /// <returns></returns>
        public Task<WafclastMonster> GetNewMonsterAsync()
          => Session.Database.CollectionMonsters.AsQueryable().Sample(1).Where(x => x.FloorLevel <= Character.CurrentFloor).FirstOrDefaultAsync();

        /// <summary>
        /// Somente usar em uma sessão.
        /// <para>Retorna um item que está no inventário do personagem.</para>
        /// </summary>
        /// <param name="name">Nome do Item</param>
        /// <returns>Item</returns>
        public Task<WafclastBaseItem> GetItemAsync(string name)
           => Session.Database.CollectionItems.Find(Session.Session, x => x.PlayerId == Id && x.Name == name,
                 new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();

        public async Task SaveItemAsync(WafclastBaseItem item)
        {
            if (item.Quantity == 0)
                await Session.RemoveAsync(item);
            else
                await Session.ReplaceAsync(item);
        }

        /// <summary>
        /// Somente usar em uma sessão.
        /// <para>Adiciona um item no inventário do personagem.</para>
        /// </summary>
        /// <param name="item">Item a ser adicionado.</param>
        /// <param name="quantity">Quantidade a ser adicionada.</param>
        /// <returns></returns>
        public async Task AddItemAsync(WafclastBaseItem item, ulong quantity)
        {
            var foundItem = await GetItemAsync(item.Name);
            if (foundItem == null)
            {
                item.Id = ObjectId.GenerateNewId();
                item.PlayerId = Id;
                item.Quantity = quantity;
                await Session.InsertAsync(item);
                return;
            }

            foundItem.Quantity += quantity;
            await Session.ReplaceAsync(foundItem);
        }

        /// <summary>
        /// Somente usar em uma sessão.
        /// <para>Salva o personagem.</para>
        /// </summary>
        /// <returns></returns>
        public Task SaveAsync()
            => Session.Database.CollectionPlayers.ReplaceOneAsync(Session.Session, x => x.Id == Id, this, new ReplaceOptions { IsUpsert = true });
    }
}
