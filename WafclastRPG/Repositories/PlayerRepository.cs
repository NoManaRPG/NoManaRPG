using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Context;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Exceptions;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Repositories {
  public class PlayerRepository : IPlayerRepository {
    private readonly MongoDbContext _context;
    private IClientSessionHandle _session;

    public PlayerRepository(MongoDbContext context) {
      _context = context;
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

    public async Task<Player> FindPlayerOrDefaultAsync(DiscordUser user)
                  => await _context.Players.Find(_session, x => x.Id == user.Id).FirstOrDefaultAsync();
    public async Task<Player> FindPlayerOrDefaultAsync(CommandContext ctx)
       => await _context.Players.Find(_session, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
    public async Task<Player> FindPlayerOrDefaultAsync(ulong id)
       => await _context.Players.Find(_session, x => x.Id == id).FirstOrDefaultAsync();

    public Task SavePlayerAsync(Player jogador)
      => _context.Players.ReplaceOneAsync(_session, x => x.Id == jogador.Id, jogador, new ReplaceOptions { IsUpsert = true });
    public async Task<object> StartSession() {
      _session = await _context.Client.StartSessionAsync();
      return new SessionHandler(_session);
    }
  }
}
