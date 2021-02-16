using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading;
using System.Threading.Tasks;

namespace WafclastRPG.Bot.Events
{
    public class GuildMemberRemoved
    {
        public static Task Event(DiscordClient c, GuildMemberRemoveEventArgs e, BotInfo botInfo)
        {
            Interlocked.Decrement(ref botInfo.Membros);
            return Task.CompletedTask;
        }
    }
}