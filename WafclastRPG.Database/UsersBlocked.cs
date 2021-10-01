using DSharpPlus.CommandsNext;
using System.Collections.Concurrent;

namespace WafclastRPG.Database {
  public class UsersBlocked {
    private readonly ConcurrentDictionary<ulong, bool> _usersBlocked;

    public UsersBlocked() {
      _usersBlocked = new ConcurrentDictionary<ulong, bool>();
    }

    public bool IsUserBlocked(ulong userId) => _usersBlocked.TryGetValue(userId, out _);
    public void UnblockUser(ulong userId) => _usersBlocked.TryRemove(userId, out _);
    public void UnblockUser(CommandContext ctx) => _usersBlocked.TryRemove(ctx.User.Id, out _);
    public void BlockUser(ulong userId) => _usersBlocked.TryAdd(userId, true);
    public void BlockUser(CommandContext ctx) => _usersBlocked.TryAdd(ctx.User.Id, true);
  }
}
