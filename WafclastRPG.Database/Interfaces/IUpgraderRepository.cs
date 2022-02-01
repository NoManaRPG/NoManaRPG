// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game;
using WafclastRPG.Game.Enuns;

namespace WafclastRPG.Database.Interfaces
{
    public interface IUpgraderRepository
    {
        Task<RankUpgrader> FindUpgraderAsync(int rank, TypeUpgrader typeUp);
        Task<RankUpgrader> FindUpgraderOrDefaultAsync(int rank, TypeUpgrader typeUp);
        Task SaveUpgraderAsync(RankUpgrader upgrader);
    }
}
