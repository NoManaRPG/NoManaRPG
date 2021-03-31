using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;

namespace WafclastRPG.Extensions
{
    public static class CommandContextExtension
    {
        public static Task ResponderNegritoAsync(this CommandContext ctx, string mensagem)
            => ctx.RespondAsync(Formatter.Bold(mensagem));

        public static Task<DiscordMessage> ResponderAsync(this CommandContext ctx, string mensagem)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}");

        public static Task ResponderAsync(this CommandContext ctx, string mensagem, DiscordEmbed embed)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}", embed);

        public static Task ResponderAsync(this CommandContext ctx, DiscordEmbed embed)
            => ctx.RespondAsync(ctx.User.Mention, embed: embed);

        public static async Task<string> WaitForMessageContentAsync(this CommandContext ctx, TimeSpan? timeoutoverride = null)
        {
            var vity = ctx.Client.GetInteractivity();
            var msg = await vity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id, timeoutoverride: timeoutoverride);
            return msg.Result.Content;
        }
    }
}
