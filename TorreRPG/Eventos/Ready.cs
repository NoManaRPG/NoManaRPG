﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TorreRPG.Eventos
{
    public static class Ready
    {
        public static Task Event(DiscordClient client, ReadyEventArgs e)
        {
            client.Logger.Log(LogLevel.Information, "Dragon", "Cliente está pronto.", DateTime.Now);
            client.UpdateStatusAsync(new DiscordActivity($"!ajuda", ActivityType.Playing), UserStatus.Online);
            return Task.CompletedTask;
        }
    }
}
