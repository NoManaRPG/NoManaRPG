using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;

namespace WafclastRPG.Extensions
{
    public static class CommandContextExtension
    {
        public static Task ResponderNegritoAsync(this CommandContext ctx, string mensagem)
            => ctx.RespondAsync(Formatter.Bold(mensagem));

        public static Task ResponderNegritoAsync(this CommandContext ctx, string mensagem, DiscordEmbed embed)
           => ctx.RespondAsync(Formatter.Bold(mensagem), embed);

        public static Task<DiscordMessage> ResponderAsync(this CommandContext ctx, string mensagem)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}");

        public static Task ResponderAsync(this CommandContext ctx, string mensagem, DiscordEmbed embed)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}", embed);

        public static Task ResponderAsync(this CommandContext ctx, DiscordEmbed embed)
            => ctx.RespondAsync(ctx.User.Mention, embed: embed);

        public static async Task<InteractivityResult<DiscordMessage>> WaitForMessageAsync(this CommandContext ctx, string message, TimeSpan? timeoutoverride = null)
        {
            var vity = ctx.Client.GetInteractivity();
            await ctx.ResponderNegritoAsync(message);
            return await vity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id, timeoutoverride: timeoutoverride);
        }

        public static async Task<InteractivityResult<DiscordMessage>> WaitForMessageAsync(this CommandContext ctx, string message, DiscordEmbed embed, TimeSpan? timeoutoverride = null)
        {
            var vity = ctx.Client.GetInteractivity();
            await ctx.ResponderNegritoAsync(message, embed);
            return await vity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id, timeoutoverride: timeoutoverride);
        }

        public static async Task<T> WaitForEnumAsync<T>(this CommandContext ctx, string message, TimeSpan? timeoutoverride = null) where T : Enum
        {
            var wait = await WaitForMessageAsync(ctx, message, timeoutoverride);
            int.TryParse(wait.Result.Content, out int result);
            T conv = (T)Enum.ToObject(typeof(T), result);
            return conv;
        }

        public static async Task<int> WaitForIntAsync(this CommandContext ctx, string message, TimeSpan? timeoutoverride = null)
        {
            var wait = await WaitForMessageAsync(ctx, message, timeoutoverride);
            int.TryParse(wait.Result.Content, out int result);
            return result;
        }

        public static async Task<ulong> WaitForUlongAsync(this CommandContext ctx, string message, TimeSpan? timeoutoverride = null)
        {
            var wait = await WaitForMessageAsync(ctx, message, timeoutoverride);
            ulong.TryParse(wait.Result.Content, out ulong result);
            return result;
        }

        public static async Task<string> WaitForStringAsync(this CommandContext ctx, string message, TimeSpan? timeoutoverride = null)
        {
            var wait = await WaitForMessageAsync(ctx, message, timeoutoverride);
            return wait.Result.Content;
        }

        public static async Task<string> WaitForStringAsync(this CommandContext ctx, string message, DiscordEmbed embed, TimeSpan? timeoutoverride = null)
        {
            var wait = await WaitForMessageAsync(ctx, message, embed, timeoutoverride);
            return wait.Result.Content;
        }
    }
}
