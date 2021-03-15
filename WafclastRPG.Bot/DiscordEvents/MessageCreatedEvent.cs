using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game;

namespace WafclastRPG.Bot.DiscordEvents
{
    public class MessageCreatedEvent
    {
        public static async Task Event(DiscordClient c, MessageCreateEventArgs e, CommandsNextExtension commandsNext, Database database)
        {
            if (e.Author.IsBot)
                return;

            if (e.Message.Content == "<@!732598033962762402>")
            {
                var cmd = commandsNext.FindCommand("info", out var customArgs);
                var fakeContext = commandsNext.CreateFakeContext(e.Author, e.Channel, "", "", cmd, customArgs);
                await commandsNext.ExecuteCommandAsync(fakeContext);
            }

            Random rd = new Random();
            using var session = await database.StartDatabaseSessionAsync();
            await session.WithTransactionAsync(async (s, ct) =>
            {
                //Find player
                var player = await session.FindPlayerAsync(e.Author.Id);
                if (player == null)
                    return Task.CompletedTask;

                if (player.Character.RegenDate > DateTime.UtcNow)
                    return Task.CompletedTask;
                player.Character.RegenDate = DateTime.UtcNow + TimeSpan.FromSeconds(rd.Sortear(90, 120));

                player.Character.ReceberVida(player.Character.Atributo.Vitalidade * 0.2m);
                await player.SaveAsync();
                return Task.CompletedTask;
            });
        }
    }
}
