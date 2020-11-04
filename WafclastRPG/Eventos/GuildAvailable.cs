using WafclastRPG.Game.Extensoes;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WafclastRPG.Game.Config;

namespace WafclastRPG.Game.Eventos
{
    public static class GuildAvailable
    {
        public static Task Event(DiscordClient client, GuildCreateEventArgs e, BotInfo botInfo)
        {
            client.Logger.LogInformation(new EventId(603, "Nova guilda"), $"Guilda {e.Guild.Name.RemoverAcentos()}", DateTime.Now);
            botInfo.Membros += e.Guild.MemberCount;
            botInfo.Guildas++;
            return Task.CompletedTask;
        }
    }
}
