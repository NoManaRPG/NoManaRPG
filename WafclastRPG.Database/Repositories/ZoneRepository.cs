// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Database.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoSession _session;

        public ZoneRepository(MongoDbContext context, IMongoSession session)
        {
            this._context = context;
            this._session = session;
        }

        public Task<WafclastRoom> FindPlayerHighestZoneAsync(ulong playerId)
        {
            if (this._session.Session != null)
                return this._context.Zones.Find(this._session.Session, x => x.PlayerId == playerId).Sort(Builders<WafclastRoom>.Sort.Ascending(x => x.Level)).Limit(1).FirstOrDefaultAsync();
            return this._context.Zones.Find(x => x.PlayerId == playerId).Sort(Builders<WafclastRoom>.Sort.Ascending(x => x.Level)).Limit(1).FirstOrDefaultAsync();
        }

        public Task<WafclastRoom> FindPlayerZoneAsync(ulong playerId, int level)
        {
            if (this._session.Session != null)
                return this._context.Zones.Find(this._session.Session, x => x.PlayerId == playerId && x.Level == level).FirstOrDefaultAsync();
            return this._context.Zones.Find(x => x.PlayerId == playerId && x.Level == level).FirstOrDefaultAsync();

        }

        public Task SaveZoneAsync(WafclastRoom room)
        {
            if (this._session.Session != null)
                return this._context.Zones.ReplaceOneAsync(this._session.Session, x => x.PlayerId == room.PlayerId, room, new ReplaceOptions { IsUpsert = true });
            return this._context.Zones.ReplaceOneAsync(x => x.PlayerId == room.PlayerId, room, new ReplaceOptions { IsUpsert = true });
        }
    }
}
