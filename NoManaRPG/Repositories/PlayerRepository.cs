// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using NoManaRPG.Exceptions;
using NoManaRPG.Entities;

namespace NoManaRPG.Repositories;

public class PlayerRepository
{
    private readonly MongoDbContext _context;
    private readonly MongoSession _session;

    public PlayerRepository(MongoDbContext context, MongoSession session)
    {
        this._context = context;
        this._session = session;
    }

    public async Task<Player> FindPlayerAsync(ulong id)
    {
        var player = await this.FindPlayerOrDefaultAsync(id);
        if (player == null)
            throw new PlayerNotCreatedException();
        return player;
    }
    public async Task<Player> FindPlayerOrDefaultAsync(ulong id)
    {
        if (this._session.Session != null)
            return await this._context.Players.Find(this._session.Session, x => x.DiscordId == id).FirstOrDefaultAsync();
        return await this._context.Players.Find(x => x.DiscordId == id).FirstOrDefaultAsync();
    }
    public Task SavePlayerAsync(Player player)
    {
        ReplaceOptions options = new() { IsUpsert = true };
        if (this._session.Session != null)
            return this._context.Players.ReplaceOneAsync(this._session.Session, x => x.DiscordId == player.DiscordId, player, options);
        return this._context.Players.ReplaceOneAsync(x => x.DiscordId == player.DiscordId, player, options);
    }
}
