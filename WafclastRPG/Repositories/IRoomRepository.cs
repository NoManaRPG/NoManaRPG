using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Wafclast;

namespace WafclastRPG.Repositories {
  public interface IRoomRepository {
    Task<Room> FindRoomOrDefaultAsync(Player player);
    Task<Room> FindRoomOrDefaultAsync(ulong id);
    Task<Room> FindRoomOrDefaultAsync(string name);
    Task SaveRoomAsync(Room item);
  }
}
