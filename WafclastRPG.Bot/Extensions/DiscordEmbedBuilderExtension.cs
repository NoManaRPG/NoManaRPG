using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace WafclastRPG.Bot.Extensions
{
    public static class DiscordEmbedBuilderExtension
    {
        public static DiscordEmbedBuilder NewEmbed(this DiscordEmbedBuilder embed, DiscordUser user)
        {
            embed.WithAuthor(user.Username, iconUrl: user.AvatarUrl);
            return embed;
        }

        public static DiscordEmbedBuilder NewEmbed(this DiscordEmbedBuilder embed, CommandContext ctx)
            => NewEmbed(embed, ctx.User);
    }
}
