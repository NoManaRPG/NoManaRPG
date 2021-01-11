using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using WafclastRPG.Bot.Extensoes;

namespace WafclastRPG.Bot.Events
{
    public static class GuildAvailable
    {
        public static Task Event(DiscordClient client, GuildCreateEventArgs e, BotInfo botInfo)
        {
            client.Logger.LogInformation(new EventId(603, "Nova guilda"), $"Guilda {e.Guild.Name.RemoverAcentos()} : {e.Guild.MemberCount} Membros.", DateTime.Now);
            Interlocked.Add(ref botInfo.Membros, e.Guild.MemberCount);
            Interlocked.Increment(ref botInfo.Guildas);
            return Task.CompletedTask;
        }
    }
}
