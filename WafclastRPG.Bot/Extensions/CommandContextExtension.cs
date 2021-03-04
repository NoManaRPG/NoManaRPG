using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Extensions
{
    public static class CommandContextExtension
    {
        public static Task ResponderAsync(this CommandContext ctx, string mensagem)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}");

        public static Task ResponderAsync(this CommandContext ctx, DiscordEmbed embed)
            => ctx.RespondAsync(ctx.User.Mention, embed: embed);

        public static async Task<bool> HasPlayerAsync(this CommandContext ctx, WafclastPlayer player)
        {
            if (player != null)
                return true;
            await ctx.ResponderAsync($"você precisa usar o comando {Formatter.InlineCode("comecar")} antes!");
            return false;
        }
    }
}
