// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using MongoDB.Driver;
using NoManaRPG.Entidades;

namespace NoManaRPG.Database.Repositories;

public class PlayerRepository
{
    private readonly DbContext _context;
    private readonly MongoSession _session;

    public PlayerRepository(DbContext context, MongoSession session)
    {
        this._context = context;
        this._session = session;
    }

    public async Task<Jogador> FindPlayerOrNullAsync(ulong id)
    {
        if (this._session.Session != null)
            return await this._context.Players.Find(this._session.Session, x => x.DiscordId == id).FirstOrDefaultAsync();
        return await this._context.Players.Find(x => x.DiscordId == id).FirstOrDefaultAsync();
    }

    public Task SavePlayerAsync(Jogador player)
    {
        ReplaceOptions options = new() { IsUpsert = true };
        if (this._session.Session != null)
            return this._context.Players.ReplaceOneAsync(this._session.Session, x => x.DiscordId == player.DiscordId, player, options);
        return this._context.Players.ReplaceOneAsync(x => x.DiscordId == player.DiscordId, player, options);
    }
}
