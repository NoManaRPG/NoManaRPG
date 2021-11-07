// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database.Interfaces
{
    public interface IZoneRepository
    {
        Task<WafclastZone> FindPlayerZoneAsync(ulong playerId, int level);
        Task<WafclastZone> FindPlayerHighestZoneAsync(ulong playerId);
        Task SaveZoneAsync(WafclastZone item);
    }
}
