// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Itens;

namespace WafclastRPG.Database.Interfaces
{
    public interface IItemRepository
    {
        Task<WafclastBaseItem> FindItemOrDefaultAsync(int globalItemId, ulong playerId);
        Task<WafclastBaseItem> FindItemOrDefaultAsync(string itemName, ulong playerId);
        Task SaveItemAsync(WafclastBaseItem item);
        Task RemoveItemAsync(WafclastBaseItem item);
    }
}
