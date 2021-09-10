using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Globalization;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Characters;

namespace WafclastRPG.Entities
{
    [BsonIgnoreExtraElements]
    public class WafclastPlayer
    {
        [BsonId]
        public ulong Id { get; private set; }
        public DateTime DateAccountCreation { get; private set; }
        public WafclastBaseCharacter Character { get; private set; }

        public ulong MonsterKills { get; set; }
        public ulong Deaths { get; set; }

        public string Language { get; set; } = "pt-BR";

        public WafclastPlayer(ulong id, WafclastBaseCharacter character)
        {
            this.Id = id;
            this.Character = character;
            DateAccountCreation = DateTime.UtcNow;
        }

        public string Mention { get => $"<@{Id.ToString(CultureInfo.InvariantCulture)}>"; }

        #region Data
        [BsonIgnore]
        public DatabaseSession dataSession;

        [BsonIgnore]
        private ReplaceOptions replace = new ReplaceOptions { IsUpsert = true };
        #endregion

        /// <summary>
        /// Somente usar em uma sessão.
        /// <para>Atualiza o personagem atual no banco de dados.</para>
        /// </summary>
        /// <returns></returns>
        public Task SaveAsync()
            => dataSession.Database.CollectionPlayers.ReplaceOneAsync(dataSession.Session, x => x.Id == Id, this, replace);

        ///// <summary>
        ///// Somente usar em uma sessão.
        ///// <para>Retorna um item que está no inventário do personagem.</para>
        ///// </summary>
        ///// <param name="name">Nome do Item</param>
        ///// <returns>Item</returns>
        //public Task<WafclastBaseItem> GetItemAsync(string name)
        //       => dataSession.Database.CollectionItems.Find(dataSession.Session, x => x.PlayerId == Id && x.Name == name,
        //             new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();

        //public async Task SaveItemAsync(WafclastBaseItem item)
        //{
        //    if (item.Quantity == 0)
        //        await dataSession.RemoveAsync(item);
        //    else
        //        await dataSession.ReplaceAsync(item);
        //}

        ///// <summary>
        ///// Somente usar em uma sessão.
        ///// <para>Adiciona um item no banco do personagem.</para>
        ///// </summary>
        ///// <param name="item">Item a ser adicionado.</param>
        ///// <param name="quantity">Quantidade a ser adicionada.</param>
        ///// <returns></returns>
        //public async Task AddItemAsync(WafclastBaseItem item, ulong quantity)
        //{
        //    var foundItem = await GetItemAsync(item.Name);
        //    if (foundItem == null)
        //    {
        //        item.Id = ObjectId.GenerateNewId();
        //        item.PlayerId = Id;
        //        item.Quantity = quantity;
        //        await dataSession.InsertAsync(item);
        //        return;
        //    }

        //    foundItem.Quantity += quantity;
        //    await dataSession.ReplaceAsync(foundItem);
        //}

        //public Task<WafclastRegion> GetRegionAsync()
        //    => Session.Database.CollectionRegions.Find(x => x.Id == Character.RegionId).FirstOrDefaultAsync();
    }
}
