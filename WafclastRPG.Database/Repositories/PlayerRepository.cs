// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MongoDB.Driver;
using WafclastRPG.Database.Exceptions;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Database.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoSession _session;
        private readonly ReplaceOptions _options = new() { IsUpsert = true };

        public PlayerRepository(MongoDbContext context, IMongoSession session)
        {
            this._context = context;
            this._session = session;
        }

        public async Task<WafclastPlayer> FindPlayerAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await this.FindPlayerOrDefaultAsync(ctx);
            if (player == null)
                throw new PlayerNotCreatedException();
            return player;
        }
        public async Task<WafclastPlayer> FindPlayerAsync(ulong id)
        {
            var player = await this.FindPlayerOrDefaultAsync(id);
            if (player == null)
                throw new PlayerNotCreatedException();
            return player;
        }
        public Task<WafclastPlayer> FindPlayerAsync(DiscordUser user) => this.FindPlayerAsync(user.Id);

        public async Task<WafclastPlayer> FindPlayerOrDefaultAsync(ulong id)
        {
            if (this._session.Session != null)
                return await this._context.Players.Find(this._session.Session, x => x.Id == id).FirstOrDefaultAsync();
            return await this._context.Players.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        public Task<WafclastPlayer> FindPlayerOrDefaultAsync(DiscordUser user) => this.FindPlayerOrDefaultAsync(user.Id);
        public Task<WafclastPlayer> FindPlayerOrDefaultAsync(CommandContext ctx) => this.FindPlayerOrDefaultAsync(ctx.User.Id);

        public Task SavePlayerAsync(WafclastPlayer player)
        {

            if (this._session.Session != null)
                return this._context.Players.ReplaceOneAsync(this._session.Session, x => x.Id == player.Id, player, this._options);
            return this._context.Players.ReplaceOneAsync(x => x.Id == player.Id, player, this._options);
        }
    }
}
