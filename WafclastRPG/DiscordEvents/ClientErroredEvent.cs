﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WafclastRPG.DiscordEvents
{
    public static class ClientErroredEvent
    {
        public static Task Event(DiscordClient client, ClientErrorEventArgs e)
        {
            string erro = $"{e.Exception.GetType()}: {e.Exception.Message}";
            client.Logger.LogError(new EventId(602, "Client Error"), erro, DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
