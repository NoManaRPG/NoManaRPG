using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class MoneyRankCommand : BaseCommandModule
    {
        public Database banco;

        [Command("rank-moedas")]
        [Aliases("rm")]
        [Description("Permite ver os 10 mais ricos.")]
        [Usage("rank-moedas")]
        public async Task MoneyRankCommandAsync(CommandContext ctx)
        {
            var timer = new Stopwatch();
            timer.Start();

            await ctx.TriggerTypingAsync();

            var f = await banco.CollectionJogadores.Find(FilterDefinition<WafclastPlayer>.Empty).Limit(10)
                .SortByDescending(x => x.Character.Coins.Coins).ToListAsync();
            var str = new StringBuilder();

            int pos = 1;
            foreach (var item in f)
            {
                str.AppendLine($"{pos}. {item.Mention()} - {item.Character.Coins.ToString()}");
                pos++;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle("Maiores acumuladores.");
            embed.WithDescription(str.ToString());
            embed.WithThumbnail("https://cdn.discordapp.com/attachments/826444525953220650/826444620815794186/podio.png");

            timer.Stop();
            embed.WithFooter($"Tempo de resposta: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.");
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
