// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Game.Entities;

namespace NoManaRPG.Extensions;

public static class PlayerRepositoryExtension
{
    public static async Task<Player> FindPlayerAsync(this PlayerRepository playerRepository, CommandContext ctx)
    {
        await ctx.TriggerTypingAsync();
        return await playerRepository.FindPlayerAsync(ctx.User.Id);
    }
    public static Task<Player> FindPlayerAsync(this PlayerRepository playerRepository, DiscordUser user)
        => playerRepository.FindPlayerAsync(user.Id);

    public static Task<Player> FindPlayerOrDefaultAsync(this PlayerRepository playerRepository, DiscordUser user)
        => playerRepository.FindPlayerOrDefaultAsync(user.Id);
    public static async Task<Player> FindPlayerOrDefaultAsync(this PlayerRepository playerRepository, CommandContext ctx)
    {
        await ctx.TriggerTypingAsync();
        return await playerRepository.FindPlayerOrDefaultAsync(ctx.User.Id);
    }
}
