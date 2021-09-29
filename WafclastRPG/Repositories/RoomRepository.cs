using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Context;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Repositories {
  public class RoomRepository : IRoomRepository {
    private readonly MongoDbContext _context;
    private readonly FindOptions _options = new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) };

    public RoomRepository(MongoDbContext context) {
      _context = context;
    }

    public Task<Room> FindRoomOrDefaultAsync(Player player) => FindRoomOrDefaultAsync(player.Character.Room.Id);
    public Task<Room> FindRoomOrDefaultAsync(ulong id) => _context.Rooms.Find(x => x.Id == id).FirstOrDefaultAsync();
    public Task<Room> FindRoomOrDefaultAsync(string name) => _context.Rooms.Find(x => x.Name == name, _options).FirstOrDefaultAsync();


    public Task SaveRoomAsync(Room room) => _context.Rooms.ReplaceOneAsync(x => x.Id == room.Id, room, new ReplaceOptions { IsUpsert = true });
  }
}
