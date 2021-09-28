using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Context;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Repositories {
  public class ItemRepository : IItemRepository {
    private readonly MongoDbContext _context;
    private readonly IMongoSession _session;

    public ItemRepository(MongoDbContext context, IMongoSession session) {
      _context = context;
      _session = session;
    }

    public Task<WafclastBaseItem> FindItemOrDefaultAsync(int globalItemId, DiscordUser user) {
      if (_session.Session != null)
        return _context.Items.Find(_session.Session, x => x.GlobalItemId == globalItemId && x.PlayerId == user.Id).FirstOrDefaultAsync();
      return _context.Items.Find(x => x.GlobalItemId == globalItemId && x.PlayerId == user.Id).FirstOrDefaultAsync();
    }
    public Task<WafclastBaseItem> FindItemOrDefaultAsync(string itemName, DiscordUser user) {
      if (_session.Session != null)
        return _context.Items.Find(_session.Session, x => x.Name == itemName && x.PlayerId == user.Id, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
      return _context.Items.Find(x => x.Name == itemName && x.PlayerId == user.Id, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
    }
    public Task SaveItemAsync(WafclastBaseItem item) {
      if (_session.Session != null)
        return _context.Items.ReplaceOneAsync(_session.Session, x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
      return _context.Items.ReplaceOneAsync(x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
    }
    public Task RemoveItemAsync(WafclastBaseItem item) {
      if (_session.Session != null)
        return _context.Items.DeleteOneAsync(_session.Session, x => x.Id == item.Id);
      return _context.Items.DeleteOneAsync(x => x.Id == item.Id);
    }
  }
}
