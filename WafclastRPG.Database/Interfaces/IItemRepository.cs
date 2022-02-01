// This file is part of WafclastRPG project.

using System.Threading.Tasks;

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
