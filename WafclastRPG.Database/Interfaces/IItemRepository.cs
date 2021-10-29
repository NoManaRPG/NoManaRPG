// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Database.Interfaces
{
    public interface IItemRepository
    {
        Task<WafclastItem> FindItemOrDefaultAsync(int globalItemId, ulong playerId);
        Task<WafclastItem> FindItemOrDefaultAsync(string itemName, ulong playerId);
        Task SaveItemAsync(WafclastItem item);
        Task RemoveItemAsync(WafclastItem item);
    }
}
