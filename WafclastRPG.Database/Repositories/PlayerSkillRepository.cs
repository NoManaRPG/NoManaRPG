// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Game.Entities.Skills;
using WafclastRPG.Database.Extensions;

namespace WafclastRPG.Database.Repositories
{
    public class PlayerSkillRepository : IPlayerSkillRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoSession _session;

        public PlayerSkillRepository(MongoDbContext context, IMongoSession session)
        {
            this._context = context;
            this._session = session;
        }

        public Task<WafclastPlayerSkill> FindSkillOrDefaultAsync(string name, ulong playerId)
        {
            if (this._session.Get() != null)
                return this._context.Skills.Find(_session.Get(), x => x.Name == name && x.PlayerId == playerId, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
            return this._context.Skills.Find(x => x.Name == name && x.PlayerId == playerId, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
        }

        public async IAsyncEnumerable<WafclastPlayerSkill> GetAllSkillAsync(ulong playerId)
        {
            IAsyncCursor<WafclastPlayerSkill> cursorSource = null;
            if (this._session.Get() != null)
            {
                cursorSource = await this._context.Skills.FindAsync(this._session.Get(), x => x.PlayerId == playerId);
                await foreach (var item in cursorSource.ToEnumerableAsync())
                    yield return item;
            }
            cursorSource = await this._context.Skills.FindAsync(x => x.PlayerId == playerId);
            await foreach (var item in cursorSource.ToEnumerableAsync())
                yield return item;
        }

        public Task SaveSkillAsync(WafclastPlayerSkill skill)
        {
            ReplaceOptions options = new() { IsUpsert = true };
            if (this._session.Get() != null)
                return this._context.Skills.ReplaceOneAsync(this._session.Get(), x => x.Name == skill.Name && x.PlayerId == skill.PlayerId, skill, options);
            return this._context.Skills.ReplaceOneAsync(x => x.Name == skill.Name && x.PlayerId == skill.PlayerId, skill, options);
        }
    }
}
