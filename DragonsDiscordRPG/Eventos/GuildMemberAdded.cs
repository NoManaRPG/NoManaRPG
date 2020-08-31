using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class GuildMemberAdded
    {
        public static Task Event(GuildMemberAddEventArgs e)
        {
            DiscordRole role = e.Guild.GetRole(742785152933036174);
            e.Member.GrantRoleAsync(role, "Entrou no servidor");
            return Task.CompletedTask;
        }
    }
}
