using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Context;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Exceptions;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Repositories {
  public class PlayerRepository : IPlayerRepository {
    private readonly MongoDbContext _context;
    private readonly IMongoSession _session;

    public PlayerRepository(MongoDbContext context, IMongoSession session) {
      _context = context;
      _session = session;
    }

    public async Task<Player> FindPlayerAsync(DiscordUser user) {
      var player = await FindPlayerOrDefaultAsync(user);
      if (player == null)
        throw new PlayerNotCreatedException();
      return player;
    }
    public async Task<Player> FindPlayerAsync(CommandContext ctx) {
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

    public async Task<Player> FindPlayerOrDefaultAsync(DiscordUser user) {
      if (_session.Session != null)
        return await _context.Players.Find(_session.Session, x => x.Id == user.Id).FirstOrDefaultAsync();
      return await _context.Players.Find(x => x.Id == user.Id).FirstOrDefaultAsync();
    }
    public async Task<Player> FindPlayerOrDefaultAsync(CommandContext ctx) {
      if (_session.Session != null)
        return await _context.Players.Find(_session.Session, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
      return await _context.Players.Find(x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
    }
    public async Task<Player> FindPlayerOrDefaultAsync(ulong id) {
      if (_session.Session != null)
        return await _context.Players.Find(_session.Session, x => x.Id == id).FirstOrDefaultAsync();
      return await _context.Players.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public Task SavePlayerAsync(Player jogador) {
      if (_session.Session != null)
        return _context.Players.ReplaceOneAsync(_session.Session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });
      return _context.Players.ReplaceOneAsync(x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });
    }
  }
}
