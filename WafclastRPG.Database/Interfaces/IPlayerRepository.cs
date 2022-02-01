// This file is part of WafclastRPG project.

using System.Threading.Tasks;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Database.Interfaces
{
    public interface IPlayerRepository
    {
        Task<WafclastPlayer> FindPlayerAsync(ulong id);
        Task<WafclastPlayer> FindPlayerOrDefaultAsync(ulong id);
        Task SavePlayerAsync(WafclastPlayer jogador);
    }
}
