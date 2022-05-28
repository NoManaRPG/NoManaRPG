// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game;

namespace WafclastRPG.Database.Interfaces
{
    public interface IUpgraderRepository
    {

        Task SaveUpgraderAsync(RankUpgrader upgrader);
    }
}
