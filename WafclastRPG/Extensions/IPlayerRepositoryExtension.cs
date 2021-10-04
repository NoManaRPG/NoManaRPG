// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Extensions
{
    public static class IPlayerRepositoryExtension
    {
        public static async Task<WafclastPlayer> FindPlayerAsync(this IPlayerRepository playerRepository, CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            return await playerRepository.FindPlayerAsync(ctx.User.Id);
        }
        public static Task<WafclastPlayer> FindPlayerAsync(this IPlayerRepository playerRepository, DiscordUser user)
            => playerRepository.FindPlayerAsync(user.Id);

        public static Task<WafclastPlayer> FindPlayerOrDefaultAsync(this IPlayerRepository playerRepository, DiscordUser user)
            => playerRepository.FindPlayerOrDefaultAsync(user.Id);
        public static async Task<WafclastPlayer> FindPlayerOrDefaultAsync(this IPlayerRepository playerRepository, CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            return await playerRepository.FindPlayerOrDefaultAsync(ctx.User.Id);
        }
    }
}
