

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Entities;
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
        public async Task Sessao(CommandContext ctx)
        {
            var can = new CancellationTokenSource();
            await Task.Delay(new Random().Next(1000, 5000));
            using (var session = await this.banco.Client.StartSessionAsync(cancellationToken: can.Token))
            {
                await session.WithTransactionAsync(async (s, ct) =>
                {
                    await Task.Delay(new Random().Next(1000, 5000));
                    var user = await this.banco.Jogadores.Find(s, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
                    if (user == null)
                    {
                        await ctx.ResponderAsync("Objeto não encontrado! cancelando operação.");
                        can.Cancel();
                        return s.AbortTransactionAsync();
                    }
                    user.Character.AddLevel();
                    await this.banco.Jogadores.ReplaceOneAsync(s, x => x.Id == user.Id, user);
                    await Task.Delay(new Random().Next(1000, 5000));
                    return Task.CompletedTask;
                }, cancellationToken: can.Token);

                await session.CommitTransactionAsync();
                await ctx.ResponderAsync("alterado");
            }


            //using (var s = await this.banco.Client.StartSessionAsync())
            //{

            //    s.StartTransaction();
            //    var data = s.Client.GetDatabase("WafclastV2Debug");
            //    var Jogadores = data.GetCollection<WafclastPlayer>("WafclastPlayer");

            //    var user = await Jogadores.Find(s, x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
            //    user.Character.AddLevel();
            //    await Jogadores.ReplaceOneAsync(s, x => x.Id == user.Id, user);
            //    await Task.Delay(3000);

            //    await s.CommitTransactionAsync();

            //    await ctx.ResponderAsync("alterado");
            //}

        }

        [Command("apagar")]
        [RequireOwner]
        public async Task Apagar(CommandContext ctx)
        {
            using (var session = await this.banco.Client.StartSessionAsync())
            {
                await session.WithTransactionAsync(async (s, ct) =>
                {
                    await this.banco.Jogadores.DeleteOneAsync(s, x => x.Id == ctx.User.Id);
                    return Task.CompletedTask;
                });

                await session.CommitTransactionAsync();
                await ctx.ResponderAsync("deletado");
            }
        }

        [Command("criar")]
        [RequireOwner]
        public async Task criar(CommandContext ctx)
        {

            WafclastPlayer p = new WafclastPlayer(ctx.User.Id);
            await banco.InsertJogadorAsync(p);

            await ctx.ResponderAsync("Criado");

        }
    }
}
