using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace WafclastRPG.Bot.Commands.AdminCommands
{
    public class TestCommands : BaseCommandModule
    {
        public Database banco;

        [Command("testevalor")]
        [RequireOwner]
        public async Task CommmandEnumTest(CommandContext ctx, [RemainingText] string hab = "")
        {
            var embed = new DiscordEmbedBuilder();

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
                    embed.WithTitle(Formatter.Bold(habilidade.GetEnumDescription().Titulo()));
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

        [Command("editar")]
        [RequireOwner]
        public async Task editar(CommandContext ctx, ulong id)
        {
            Task<bool> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var user = await session.FindPlayerAsync(id);
                    if (user.Character.Level >= 4)
                        return Task.FromResult(false);
                    user.Character.Level = 10;
                    await user.SaveAsync();
                    return Task.FromResult(true);
                });
            }

            if (await result == true)
                await ctx.ResponderAsync("alterado");
            else
                await ctx.ResponderAsync("não alterado");
        }

        [Command("zerar")]
        [RequireOwner]
        public async Task asd(CommandContext ctx)
        {
            Task<bool> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var user = await session.FindPlayerAsync(ctx.User);
                    user.Character.Level = 0;
                    await user.SaveAsync();
                    return Task.FromResult(true);
                });
            };

            if (await result == true)
                await ctx.ResponderAsync("zerado");
        }
    }
}
