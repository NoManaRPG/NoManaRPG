// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Game.Entities.Skills;

namespace WafclastRPG.Extensions
{
    public static class ISkillRepositoryExtenstion
    {
        public static Task<WafclastPlayerSkill> FindSkillOrDefaultAsync(this IPlayerSkillRepository skillRepository, string name, DiscordUser user)
            => skillRepository.FindSkillOrDefaultAsync(name, user.Id);

        public static Task<WafclastPlayerSkill> FindSkillOrDefaultAsync(this IPlayerSkillRepository skillRepository, string name, CommandContext ctx)
            => skillRepository.FindSkillOrDefaultAsync(name, ctx.User.Id);
    }
}
