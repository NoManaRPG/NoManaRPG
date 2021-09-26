using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using WafclastRPG.Extensions;

namespace WafclastRPG.DiscordEvents {
  public static class GuildAvailableEvent {
    public static Task Event(DiscordClient client, GuildCreateEventArgs e) {
      client.Logger.LogInformation(new EventId(603, "Nova guilda"), $"Guilda {e.Guild.Name.RemoverAcentos()} : {e.Guild.MemberCount} Membros.", DateTime.Now);
      return Task.CompletedTask;
    }
  }
}
