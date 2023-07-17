// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Game.Entities.Items;

namespace NoManaRPG.Extensions;

public static class ItemRepositoryExtension
{
    public static Task<Item> FindItemOrDefaultAsync(this ItemRepository itemRepository, string itemName, DiscordUser user)
          => itemRepository.FindItemOrDefaultAsync(itemName, user.Id);
    public static Task<Item> FindItemOrDefaultAsync(this ItemRepository itemRepository, string itemName, DiscordMember member)
          => itemRepository.FindItemOrDefaultAsync(itemName, member.Id);
    public static Task<Item> FindItemOrDefaultAsync(this ItemRepository itemRepository, string itemName, CommandContext ctx)
          => itemRepository.FindItemOrDefaultAsync(itemName, ctx.Member.Id);
}
