// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using WafclastRPG.Database.Interfaces;

namespace WafclastRPG.Extensions
{
    public static class IItemRepositoryExtension
    {
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, int globalItemId, DiscordUser user)
            => itemRepository.FindItemOrDefaultAsync(globalItemId, user.Id);
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, int globalItemId, DiscordMember member)
            => itemRepository.FindItemOrDefaultAsync(globalItemId, member.Id);
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, int globalItemId, CommandContext ctx)
            => itemRepository.FindItemOrDefaultAsync(globalItemId, ctx.Member.Id);

        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, string itemName, DiscordUser user)
              => itemRepository.FindItemOrDefaultAsync(itemName, user.Id);
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, string itemName, DiscordMember member)
              => itemRepository.FindItemOrDefaultAsync(itemName, member.Id);
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, string itemName, CommandContext ctx)
              => itemRepository.FindItemOrDefaultAsync(itemName, ctx.Member.Id);
    }
}