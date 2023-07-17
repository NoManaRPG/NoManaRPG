// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using NoManaRPG.Game.Entities.Rooms;

namespace NoManaRPG.Database.Repositories;

public class ZoneRepository
{
    private readonly MongoDbContext _context;
    private readonly MongoSession _session;

    public ZoneRepository(MongoDbContext context, MongoSession session)
    {
        this._context = context;
        this._session = session;
    }

    public Task<Room> FindPlayerHighestZoneAsync(ulong playerId)
    {
        if (this._session.Session != null)
            return this._context.Zones.Find(this._session.Session, x => x.PlayerId == playerId).Sort(Builders<Room>.Sort.Ascending(x => x.Level)).Limit(1).FirstOrDefaultAsync();
        return this._context.Zones.Find(x => x.PlayerId == playerId).Sort(Builders<Room>.Sort.Ascending(x => x.Level)).Limit(1).FirstOrDefaultAsync();
    }

    public Task<Room> FindPlayerZoneAsync(ulong playerId, int level)
    {
        if (this._session.Session != null)
            return this._context.Zones.Find(this._session.Session, x => x.PlayerId == playerId && x.Level == level).FirstOrDefaultAsync();
        return this._context.Zones.Find(x => x.PlayerId == playerId && x.Level == level).FirstOrDefaultAsync();

    }

    public Task SaveZoneAsync(Room room)
    {
        if (this._session.Session != null)
            return this._context.Zones.ReplaceOneAsync(this._session.Session, x => x.PlayerId == room.PlayerId, room, new ReplaceOptions { IsUpsert = true });
        return this._context.Zones.ReplaceOneAsync(x => x.PlayerId == room.PlayerId, room, new ReplaceOptions { IsUpsert = true });
    }
}
