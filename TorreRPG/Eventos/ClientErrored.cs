using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TorreRPG.Eventos
{
    public static class ClientErrored
    {
        public static Task Event(ClientErrorEventArgs e)
        {
            string erro = $"{e.Exception.GetType()}: {e.Exception.Message}";
            e.Client.Logger.Log(LogLevel.Error, "Dragon", erro, DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
