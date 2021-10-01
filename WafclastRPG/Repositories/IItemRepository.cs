﻿using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Itens;

namespace WafclastRPG.Repositories {
  public interface IItemRepository {
    Task<WafclastBaseItem> FindItemOrDefaultAsync(int globalItemId, DiscordUser user);
    Task<WafclastBaseItem> FindItemOrDefaultAsync(string itemName, DiscordUser user);
    Task SaveItemAsync(WafclastBaseItem item);
    Task RemoveItemAsync(WafclastBaseItem item);
  }
}
