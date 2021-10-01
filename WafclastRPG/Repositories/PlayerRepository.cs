using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Context;
using WafclastRPG.Exceptions;
using WafclastRPG.Game.Entities.Wafclast;


namespace WafclastRPG.Repositories {
  public class PlayerRepository : IPlayerRepository {
    private readonly MongoDbContext _context;
    private readonly IMongoSession _session;
    private readonly ReplaceOptions _options = new ReplaceOptions { IsUpsert = true };

    public PlayerRepository(MongoDbContext context, IMongoSession session) {
      _context = context;
      _session = session;
    }

    public async Task<Player> FindPlayerAsync(CommandContext ctx) {
      await ctx.TriggerTypingAsync();
      var player = await FindPlayerOrDefaultAsync(ctx);
      if (player == null)
        throw new PlayerNotCreatedException();
      return player;
    }
    public async Task<Player> FindPlayerAsync(ulong id) {
      var player = await FindPlayerOrDefaultAsync(id);
      if (player == null)
        throw new PlayerNotCreatedException();
      return player;
    }
    public Task<Player> FindPlayerAsync(DiscordUser user) => FindPlayerAsync(user.Id);

    public async Task<Player> FindPlayerOrDefaultAsync(ulong id) {
      if (_session.Session != null)
        return await _context.Players.Find(_session.Session, x => x.Id == id).FirstOrDefaultAsync();
      return await _context.Players.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
    public Task<Player> FindPlayerOrDefaultAsync(DiscordUser user) => FindPlayerOrDefaultAsync(user.Id);
    public Task<Player> FindPlayerOrDefaultAsync(CommandContext ctx) => FindPlayerOrDefaultAsync(ctx.User.Id);

    public Task SavePlayerAsync(Player player) {

      if (_session.Session != null)
        return _context.Players.ReplaceOneAsync(_session.Session, x => x.Id == player.Id, player, _options);
      return _context.Players.ReplaceOneAsync(x => x.Id == player.Id, player, _options);
    }
  }
}
