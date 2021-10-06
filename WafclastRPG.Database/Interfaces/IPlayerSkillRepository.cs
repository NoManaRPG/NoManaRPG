// This file is part of the WafclastRPG project.

using System.Collections.Generic;
using System.Threading.Tasks;
using WafclastRPG.Game.Entities.Skills;

namespace WafclastRPG.Database.Interfaces
{
    public interface IPlayerSkillRepository
    {
        Task<WafclastPlayerSkill> FindSkillOrDefaultAsync(string name, ulong playerId);
        IAsyncEnumerable<WafclastPlayerSkill> GetAllSkillAsync(ulong playerId);
        Task SaveSkillAsync(WafclastPlayerSkill item);
    }
}
