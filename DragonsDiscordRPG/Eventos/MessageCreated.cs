using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class MessageCreated
    {
        public static async Task EventAsync(MessageCreateEventArgs e)
        {
            if (e.Channel.Id != 742793406417338468)
            {

                return;
            }
            DiscordMember membro = await e.Guild.GetMemberAsync(e.Author.Id);
            DiscordRole role = e.Guild.GetRole(742785152933036174);
            await membro.GrantRoleAsync(role, "Avisou que estava sem ver os canais de vozes");
            await e.Message.DeleteAsync();
            return;
        }
    }
}
