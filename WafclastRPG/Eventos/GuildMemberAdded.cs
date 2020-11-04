using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Bot.Config;

namespace WafclastRPG.Bot.Eventos
{
    public class GuildMemberAdded
    {
        public static Task Event(DiscordClient c, GuildMemberAddEventArgs e, BotInfo botInfo)
        {
            Interlocked.Increment(ref botInfo.Membros);
            return Task.CompletedTask;
        }
    }
}