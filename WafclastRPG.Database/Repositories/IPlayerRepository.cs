// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Database.Repositories
{
    public interface IPlayerRepository
    {
        Task<WafclastPlayer> FindPlayerAsync(ulong id);
        Task<WafclastPlayer> FindPlayerOrDefaultAsync(ulong id);
        Task SavePlayerAsync(WafclastPlayer jogador);
    }
}
