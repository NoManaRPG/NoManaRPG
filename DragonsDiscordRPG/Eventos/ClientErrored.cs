using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Eventos
{
    public static class ClientErrored
    {
        public static Task Event(ClientErrorEventArgs e)
        {
            string erro = $"Um erro aconteceu no client: {e.Exception.GetType()}: {e.Exception.Message}";
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "Dragon", erro, DateTime.Now);
            DiscordChannel channel = e.Client.GetChannelAsync(742778666509008956).Result;
            e.Client.SendMessageAsync(channel, erro);
            return Task.CompletedTask;
        }
    }
}
