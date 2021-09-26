using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using Emzi0767.Utilities;

namespace WafclastRPG.DiscordEvents {

  public class Pessoa {
    public ulong Id { get; set; }
  }

  public class MessageCreatedEvent {

    public Pessoa MessageCreateEvent { get; private set; } = new Pessoa();

    public MessageCreatedEvent(out AsyncEventHandler<DiscordClient, MessageCreateEventArgs> asyncEvent) {
      asyncEvent = Event;
    }

    public Task Event(DiscordClient c, MessageCreateEventArgs e) {
      if (e.Author.IsBot)
        return Task.CompletedTask;
      MessageCreateEvent.Id = e.Author.Id;
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
