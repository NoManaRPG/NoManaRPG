using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Bot.Comandos.Exibir;

namespace WafclastRPG.Bot.Events
{
    public class MessageCreated
    {
        public static async Task Event(DiscordClient c, MessageCreateEventArgs e, CommandsNextExtension commandsNext)
        {
            if (e.Message.Content == "<@!754805014261661737>")
            {
                var cmd = commandsNext.FindCommand("info", out var customArgs);
                var fakeContext = commandsNext.CreateFakeContext(e.Author, e.Channel, "", "", cmd, customArgs);
                await commandsNext.ExecuteCommandAsync(fakeContext);
                await Task.CompletedTask;
            }
            await Task.CompletedTask;
        }
    }
}
