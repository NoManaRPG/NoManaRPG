// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database.Interfaces
{
    public interface IZoneRepository
    {
        Task<WafclastZone> FindZoneOrDefaultAsync(ulong id);
        Task<WafclastZone> FindZoneOrDefaultAsync(string name);
        Task SaveZoneAsync(WafclastZone item);
    }
}
