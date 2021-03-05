using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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
            str.AppendLine($"Ataque: {player.Character.Ataque}");
            str.AppendLine($"Defesa: {player.Character.Defesa}");
            str.AppendLine($"Vida: {player.Character.VidaAtual}");
            str.AppendLine($"Vida max: {player.Character.VidaMaxima}");

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.WithTitle("Status");
            embed.WithDescription(str.ToString());


            await ctx.ResponderAsync(embed.Build());
        }
    }
}
