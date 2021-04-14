using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using WafclastRPG.Extensions;
using WafclastRPG.DataBases;

namespace WafclastRPG.DiscordEvents
{
    public class MessageCreatedEvent
    {
        public static async Task Event(DiscordClient c, MessageCreateEventArgs e, CommandsNextExtension commandsNext, DataBase database)
        {
            if (e.Author.IsBot)
                return;

            using (var session = await database.StartDatabaseSessionAsync())
                await session.Session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(e.Author);
                    if (player == null)
                        return Task.CompletedTask;

                    if (player.Character.RegenDate > DateTime.UtcNow)
                        return Task.CompletedTask;

                    Random rd = new Random();
                    player.Character.RegenDate = DateTime.UtcNow + TimeSpan.FromSeconds(rd.Sortear(90, 120));

                    player.Character.Mana.Add(player.Character.ManaRegen.MaxValue);
                    player.Character.Coins.Add(0, 0, (ulong)rd.Sortear(1, 2));
                    player.Character.Stamina.Add(rd.Sortear(1, player.Character.Level));
                    await session.ReplaceAsync(player);
                    return Task.CompletedTask;
                });
        }
    }
}
