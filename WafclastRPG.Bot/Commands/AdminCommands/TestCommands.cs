

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Bot.Extensions;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace WafclastRPG.Bot.Commands.AdminCommands
{
    public class TestCommands : BaseCommandModule
    {
        public Banco banco;

        [Command("testevalor")]
        [RequireOwner]
        public async Task CommmandEnumTest(CommandContext ctx, [RemainingText] string hab = "")
        {
            var embed = new DiscordEmbedBuilder().NewEmbed(ctx);

            if (string.IsNullOrEmpty(hab))
            {
                embed.WithDescription("Valor 1\nValor com Valor 1");
            }
            else
            {
                hab = hab.RemoverAcentos();
                hab = Regex.Replace(hab, @"\s+", "");
                if (Enum.TryParse<enumtest>(hab, true, out var habilidade))
                {
                    embed.WithTitle(habilidade.GetEnumDescription().Titulo().Bold());
                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você informou valor inexistente!");
                    return;
                }
            }

            await ctx.RespondAsync(embed: embed.Build());
        }

        public enum enumtest
        {
            [Description("Valor 1")]
            Valor1,
            [Description("Valor com Valor 1")]
            ValorComValor1
        }
    }
}
