using TorreRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace TorreRPG.Eventos
{
    public static class GuildAvailable
    {
        public static Task Event(GuildCreateEventArgs e, BotInfo botInfo)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Dragon", $"Guilda {e.Guild.Name.RemoverAcentos()}", DateTime.Now);
            botInfo.QuantidadeMembros += e.Guild.MemberCount;
            return Task.CompletedTask;
        }
    }
}
