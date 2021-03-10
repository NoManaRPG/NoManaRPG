using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Database;
using WafclastRPG.Bot.Extensions;

namespace WafclastRPG.Bot.Commands.GeneralCommands
{
    public class StatusCommand : BaseCommandModule
    {
        public BotDatabase banco;

        [Command("status")]
        [Description("Exibe o status do seu personagem.")]
        [Usage("status")]
        public async Task StatusCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx.User.Id);
            if (!await ctx.HasPlayerAsync(player))
                return;

            var str = new StringBuilder();
            str.AppendLine($"Ataque: {player.Character.Ataque:N2}");
            str.AppendLine($"Defesa: {player.Character.Defesa:N2}");
            str.AppendLine($"Vida: {player.Character.VidaAtual:N2}");
            str.AppendLine($"Vida max: {player.Character.VidaMaxima:N2}");

            var dg = await ctx.Client.GetGuildAsync(player.Character.ServerId, false);
            var dc = dg.GetChannel(player.Character.LocalId);
            var invite = await dc.CreateInviteAsync(60, 0);

            str.AppendLine(Formatter.MaskedUrl(dc.Name, new Uri(invite.ToString())));

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.WithTitle("Status");
            embed.WithDescription(str.ToString());

            await ctx.ResponderAsync(embed.Build());
        }
    }
}
