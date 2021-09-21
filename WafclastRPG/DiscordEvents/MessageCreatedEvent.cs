using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using WafclastRPG.Extensions;
using WafclastRPG.DataBases;

namespace WafclastRPG.DiscordEvents {
  public class MessageCreatedEvent {
    public static Task Event(DiscordClient c, MessageCreateEventArgs e, CommandsNextExtension commandsNext) {
      if (e.Author.IsBot)
        return Task.CompletedTask;
      return Task.CompletedTask;

      //using (var session = await database.StartDatabaseSessionAsync())
      //    await session.Session.WithTransactionAsync(async (s, ct) =>
      //    {
      //        var player = await session.FindPlayerAsync(e.Author);
      //        if (player == null)
      //            return Task.CompletedTask;

      //        if (player.Character.RegenDate > DateTime.UtcNow)
      //            return Task.CompletedTask;

      //        Random rd = new Random();
      //        player.Character.RegenDate = DateTime.UtcNow + TimeSpan.FromSeconds(rd.Sortear(90, 120));

      //        player.Character.Mana.Add(player.Character.ManaRegen.MaxValue);
      //        player.Character.Coins.Add((ulong)rd.Sortear(1, 2));
      //        await session.ReplaceAsync(player);
      //        return Task.CompletedTask;
      //    });
    }
  }
}
