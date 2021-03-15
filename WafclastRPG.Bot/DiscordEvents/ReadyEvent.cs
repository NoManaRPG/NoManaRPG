using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WafclastRPG.Bot.DiscordEvents
{
    public static class ReadyEvent
    {
        public static Task Event(DiscordClient client, ReadyEventArgs e)
        {
            client.Logger.Log(LogLevel.Information, "Cliente está pronto.", DateTime.Now);
            client.UpdateStatusAsync(new DiscordActivity($"w.ajuda | Venha me conhecer!", ActivityType.ListeningTo), UserStatus.Online);
            return Task.CompletedTask;
        }
    }
}
