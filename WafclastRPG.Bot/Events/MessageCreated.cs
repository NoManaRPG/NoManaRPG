using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WafclastRPG.Bot.Events
{
    public class MessageCreated
    {
        public static async Task Event(DiscordClient c, MessageCreateEventArgs e)
        {
            await Task.CompletedTask;
        }
    }
}
