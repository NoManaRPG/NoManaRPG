// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Game.Entities.Itens;


namespace WafclastRPG.Database.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoSession _session;

        public ItemRepository(MongoDbContext context, IMongoSession session)
        {
            this._context = context;
            this._session = session;
        }

        public Task<WafclastBaseItem> FindItemOrDefaultAsync(int globalItemId, ulong playerId)
        {
            if (this._session.Session != null)
                return this._context.Items.Find(this._session.Session, x => x.GlobalItemId == globalItemId && x.PlayerId == playerId).FirstOrDefaultAsync();
            return this._context.Items.Find(x => x.GlobalItemId == globalItemId && x.PlayerId == playerId).FirstOrDefaultAsync();
        }
        public Task<WafclastBaseItem> FindItemOrDefaultAsync(string itemName, ulong playerId)
        {
            if (this._session.Session != null)
                return this._context.Items.Find(this._session.Session, x => x.Name == itemName && x.PlayerId == playerId, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
            return this._context.Items.Find(x => x.Name == itemName && x.PlayerId == playerId, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
        }
        public Task SaveItemAsync(WafclastBaseItem item)
        {
            if (this._session.Session != null)
                return this._context.Items.ReplaceOneAsync(this._session.Session, x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
            return this._context.Items.ReplaceOneAsync(x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
        }
        public Task RemoveItemAsync(WafclastBaseItem item)
        {
            if (this._session.Session != null)
                return this._context.Items.DeleteOneAsync(this._session.Session, x => x.Id == item.Id);
            return this._context.Items.DeleteOneAsync(x => x.Id == item.Id);
        }
    }
}
