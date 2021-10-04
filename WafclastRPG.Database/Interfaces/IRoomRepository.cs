// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database.Interfaces
{
    public interface IRoomRepository
    {
        Task<WafclastRoom> FindRoomOrDefaultAsync(WafclastPlayer player);
        Task<WafclastRoom> FindRoomOrDefaultAsync(ulong id);
        Task<WafclastRoom> FindRoomOrDefaultAsync(string name);
        Task SaveRoomAsync(WafclastRoom item);
    }
}
