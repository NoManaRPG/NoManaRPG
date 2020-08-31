using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class GuildMemberAdded
    {
        public static Task Event(GuildMemberAddEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
