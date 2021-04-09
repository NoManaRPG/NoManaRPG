using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using WafclastRPG.Extensions;
using WafclastRPG;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;

namespace WafclastRPG.DiscordEvents
{
    public class MessageCreatedEvent
    {
        public static async Task Event(DiscordClient c, MessageCreateEventArgs e, CommandsNextExtension commandsNext, DataBase database)
        {
            if (e.Author.IsBot)
                return;

            if (e.Message.Content == "<@!732598033962762402>")
            {
                var cmd = commandsNext.FindCommand("info", out var customArgs);
                var fakeContext = commandsNext.CreateFakeContext(e.Author, e.Channel, "", "", cmd, customArgs);
                await commandsNext.ExecuteCommandAsync(fakeContext);
            }

            using (var session = await database.StartDatabaseSessionAsync())
                await session.Session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(e.Author);
                    if (player == null)
                    {
                        player = new WafclastPlayer(e.Author.Id, false);
                        await session.ReplaceAsync(player);
                    }

                    if (player.Character == null)
                        return Task.CompletedTask;

                    if (player.Character.RegenDate > DateTime.UtcNow)
                        return Task.CompletedTask;

                    Random rd = new Random();
                    player.Character.RegenDate = DateTime.UtcNow + TimeSpan.FromSeconds(rd.Sortear(90, 120));

                    player.Character.Mana.Add(player.Character.ManaRegen.MaxValue);
                    player.Character.Coins.Add(0, 0, (ulong)rd.Sortear(1, 2));
                    await session.ReplaceAsync(player);
                    return Task.CompletedTask;
                });
        }
    }
}
