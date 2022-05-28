// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Items;

namespace WafclastRPG.Database.Interfaces
{
    public interface IItemRepository
    {
        Task<WafclastItem> FindItemOrDefaultAsync(string itemName, ulong playerId);
        Task SaveItemAsync(WafclastItem item);
        Task RemoveItemAsync(WafclastItem item);
    }
}
