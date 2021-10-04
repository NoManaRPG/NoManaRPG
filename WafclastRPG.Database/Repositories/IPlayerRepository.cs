// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Database.Repositories
{
    public interface IPlayerRepository
    {
        Task<WafclastPlayer> FindPlayerAsync(DiscordUser user);
        Task<WafclastPlayer> FindPlayerAsync(CommandContext ctx);
        Task<WafclastPlayer> FindPlayerAsync(ulong id);

        Task<WafclastPlayer> FindPlayerOrDefaultAsync(DiscordUser user);
        Task<WafclastPlayer> FindPlayerOrDefaultAsync(CommandContext ctx);
        Task<WafclastPlayer> FindPlayerOrDefaultAsync(ulong id);
        Task SavePlayerAsync(WafclastPlayer jogador);
    }
}
