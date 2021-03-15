using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WafclastRPG.Bot.DiscordEvents
{
    public class MessageDeleted
    {
        public static async Task Event(DiscordClient c, MessageDeleteEventArgs e)
        {
            await Task.CompletedTask;
        }
    }
}
