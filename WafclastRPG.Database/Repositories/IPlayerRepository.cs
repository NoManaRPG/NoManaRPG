using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Wafclast;

namespace WafclastRPG.Database.Repositories {
  public interface IPlayerRepository {
    Task<Player> FindPlayerAsync(DiscordUser user);
    Task<Player> FindPlayerAsync(CommandContext ctx);
    Task<Player> FindPlayerAsync(ulong id);

    Task<Player> FindPlayerOrDefaultAsync(DiscordUser user);
    Task<Player> FindPlayerOrDefaultAsync(CommandContext ctx);
    Task<Player> FindPlayerOrDefaultAsync(ulong id);
    Task SavePlayerAsync(Player jogador);
  }
}
