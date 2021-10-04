// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MongoDbContext _context;
        private readonly FindOptions _options = new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) };

        public RoomRepository(MongoDbContext context)
        {
            this._context = context;
        }

        public Task<WafclastRoom> FindRoomOrDefaultAsync(WafclastPlayer player) => this.FindRoomOrDefaultAsync(player.Character.Room.Id);
        public Task<WafclastRoom> FindRoomOrDefaultAsync(ulong id) => this._context.Rooms.Find(x => x.Id == id).FirstOrDefaultAsync();
        public Task<WafclastRoom> FindRoomOrDefaultAsync(string name) => this._context.Rooms.Find(x => x.Name == name, this._options).FirstOrDefaultAsync();


        public Task SaveRoomAsync(WafclastRoom room) => this._context.Rooms.ReplaceOneAsync(x => x.Id == room.Id, room, new ReplaceOptions { IsUpsert = true });
    }
}
