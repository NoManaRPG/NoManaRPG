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
            string erro = $"{e.Exception.GetType()}: {e.Exception.Message}";
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "Dragon", erro, DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
