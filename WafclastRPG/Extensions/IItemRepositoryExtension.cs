// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Game.Entities.Items;

namespace WafclastRPG.Extensions
{
    public static class IItemRepositoryExtension
    {
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, string itemName, DiscordUser user)
              => itemRepository.FindItemOrDefaultAsync(itemName, user.Id);
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, string itemName, DiscordMember member)
              => itemRepository.FindItemOrDefaultAsync(itemName, member.Id);
        public static Task<WafclastItem> FindItemOrDefaultAsync(this IItemRepository itemRepository, string itemName, CommandContext ctx)
              => itemRepository.FindItemOrDefaultAsync(itemName, ctx.Member.Id);
    }
}
