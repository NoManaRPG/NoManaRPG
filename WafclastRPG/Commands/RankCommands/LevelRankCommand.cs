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

namespace WafclastRPG.Commands.RankCommands
{
    class LevelRankCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("rank-nivel")]
        [Aliases("rn")]
        [Description("Permite ver os 10 mais evoluidos.")]
        [Usage("rank-nivel")]
        public async Task LevelRanKCommandAsync(CommandContext ctx)
        {
            var timer = new Stopwatch();
            timer.Start();

            await ctx.TriggerTypingAsync();

            var f = await banco.CollectionPlayers.Find(x => x.Character.Localization.ServerId == ctx.Guild.Id).Limit(10)
                .SortByDescending(x => x.Character.Level).ToListAsync();
            var str = new StringBuilder();

            int pos = 1;
            foreach (var item in f)
            {
                str.AppendLine($"{pos}. {item.Mention} - Nv.{item.Character.Level}");
                pos++;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle("Maiores niveladores.");
            embed.WithDescription(str.ToString());
            embed.WithThumbnail("https://cdn.discordapp.com/attachments/826444525953220650/826444620815794186/podio.png");

            timer.Stop();
            embed.WithFooter($"Tempo de resposta: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.");
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
