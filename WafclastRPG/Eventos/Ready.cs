using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WafclastRPG.Eventos
{
    public static class Ready
    {
        public static Task Event(DiscordClient client, ReadyEventArgs e)
        {
            client.Logger.Log(LogLevel.Information, "Cliente está pronto.", DateTime.Now);
            client.UpdateStatusAsync(new DiscordActivity($"!ajuda", ActivityType.ListeningTo), UserStatus.Online);
            return Task.CompletedTask;
        }
    }
}
