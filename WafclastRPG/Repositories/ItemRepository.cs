using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Context;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Repositories {
  public class ItemRepository : IItemRepository {
    private readonly MongoDbContext _context;
    private IClientSessionHandle _session;

    public ItemRepository(MongoDbContext context) {
      _context = context;
    }

    public void Configure(object obj) {
      if (obj is null)
        return;
      if (obj is SessionHandler) {
        var sessionHandler = obj as SessionHandler;
        _session = sessionHandler.Session;
      }
    }

    public Task<WafclastBaseItem> FindItemOrDefaultAsync(int globalItemId, DiscordUser user)
        => _context.Items.Find(_session, x => x.GlobalItemId == globalItemId && x.PlayerId == user.Id).FirstOrDefaultAsync();
    public Task<WafclastBaseItem> FindItemOrDefaultAsync(string itemName, DiscordUser user)
     => _context.Items.Find(_session, x => x.Name == itemName && x.PlayerId == user.Id, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
    public Task SaveAsync(WafclastBaseItem item)
     => _context.Items.ReplaceOneAsync(_session, x => x.Id == item.Id, item, new ReplaceOptions { IsUpsert = true });
    public Task RemoveAsync(WafclastBaseItem item)
          => _context.Items.DeleteOneAsync(_session, x => x.Id == item.Id);
  }
}
