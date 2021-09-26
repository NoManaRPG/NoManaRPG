using DSharpPlus.CommandsNext;
using System.Collections.Concurrent;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Repositories {
  public class InteractivityRepository : IInteractivityRepository {
    private readonly ConcurrentDictionary<ulong, bool> _usersBlocked;

    public InteractivityRepository() {
      _usersBlocked = new ConcurrentDictionary<ulong, bool>();
    }

    public bool IsBlocked(ulong userId) => _usersBlocked.TryGetValue(userId, out _);
    public void Unblock(ulong userId) => _usersBlocked.TryRemove(userId, out _);
    public void Unblock(CommandContext ctx) => _usersBlocked.TryRemove(ctx.User.Id, out _);
    public void Block(ulong userId) => _usersBlocked.TryAdd(userId, true);
    public void Block(CommandContext ctx) => _usersBlocked.TryAdd(ctx.User.Id, true);
  }
}
