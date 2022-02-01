// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database.Interfaces
{
    public interface IZoneRepository
    {
        Task<WafclastRoom> FindPlayerZoneAsync(ulong playerId, int level);
        Task<WafclastRoom> FindPlayerHighestZoneAsync(ulong playerId);
        Task SaveZoneAsync(WafclastRoom item);
    }
}
