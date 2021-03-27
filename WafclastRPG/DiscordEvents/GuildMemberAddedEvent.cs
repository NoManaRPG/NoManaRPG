using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading;
using System.Threading.Tasks;

namespace WafclastRPG.DiscordEvents
{
    public class GuildMemberAddedEvent
    {
        public static Task Event(DiscordClient c, GuildMemberAddEventArgs e, BotInfo botInfo)
        {
            Interlocked.Increment(ref botInfo.Membros);
            return Task.CompletedTask;
        }
    }
}