// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly MongoDbContext _context;

        public ZoneRepository(MongoDbContext context)
        {
            this._context = context;
        }

        public Task<WafclastZone> FindZoneOrDefaultAsync(ulong id)
            => this._context.Zones.Find(x => x.Id == id).FirstOrDefaultAsync();
        public Task<WafclastZone> FindZoneOrDefaultAsync(string name)
            => this._context.Zones.Find(x => x.Name == name, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
        public Task SaveZoneAsync(WafclastZone room)
            => this._context.Zones.ReplaceOneAsync(x => x.Id == room.Id, room, new ReplaceOptions { IsUpsert = true });
    }
}
