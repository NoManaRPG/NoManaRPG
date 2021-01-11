using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace WafclastRPG.Bot.Extensions
{
    public static class CommandContextExtension
    {
        public static Task ResponderAsync(this CommandContext ctx, string mensagem)
            => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}");

        public static Task ResponderAsync(this CommandContext ctx, DiscordEmbed embed)
            => ctx.RespondAsync(ctx.User.Mention, embed: embed);
    }
}
