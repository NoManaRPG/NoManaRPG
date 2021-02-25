using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Bot.Database;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Entities;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace WafclastRPG.Bot.Commands.AdminCommands
{
    public class TestCommands : BaseCommandModule
    {
        public BotDatabase banco;

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

        [Command("get")]
        [RequireOwner]
        public async Task criar(CommandContext ctx, int valor)
        {
            await ctx.RespondAsync($"{ExperienceTotalLevel(valor)}");
        }
        private int ExperienceTotalLevel(int level)
        {
            double v1 = 1.0 / 8.0 * level * (level - 1.0) + 75.0;
            double pow1 = Math.Pow(2, (level - 1.0) / 7.0) - 1;
            double pow2 = 1 - Math.Pow(2, -1 / 7.0);
            return (int)Math.Truncate(v1 * (pow1 / pow2));
        }

        [Command("sessao")]
        [RequireOwner]
        public async Task Sessao(CommandContext ctx, ulong id)
        {
            string mensagem;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                mensagem = await session.WithTransactionAsync(async (s, ct) =>
                  {
                      var user = await session.FindJogadorAsync(id);
                      if (user.Character.Level >= 10)
                          return "Nivel max";

                      user.Character.AddLevel();
                      await user.SaveAsync();
                      await ctx.ResponderAsync("+1 level");
                      await Task.Delay(5000);

                      user.Character.AddLevel();
                      await user.SaveAsync();
                      await ctx.ResponderAsync("+1 level..");
                      await Task.Delay(5000);
                      return "";
                  });
            }
            await ctx.ResponderAsync(mensagem);
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
                    var user = await session.FindJogadorAsync(id);
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
        public async Task criar(CommandContext ctx)
        {
            Task<bool> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var user = await session.FindJogadorAsync(ctx.User);
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
