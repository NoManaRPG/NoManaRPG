using TorreRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TorreRPG.Config;

namespace TorreRPG.Eventos
{
    public static class GuildAvailable
    {
        public static Task Event(DiscordClient client, GuildCreateEventArgs e, BotInfo botInfo)
        {
            client.Logger.Log(LogLevel.Information, "Dragon", $"Guilda {e.Guild.Name.RemoverAcentos()}", DateTime.Now);
            botInfo.QuantidadeMembros += e.Guild.MemberCount;
            return Task.CompletedTask;
        }
    }
}
