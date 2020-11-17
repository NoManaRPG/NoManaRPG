using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoRank : BaseCommandModule
    {
        public Banco banco;

        [Command("top")]
        [Description("Exibe os 10 jogadores mais ricos.")]
        [Cooldown(1, 30, CooldownBucketType.User)]
        public async Task ComandoMochilaAsync(CommandContext ctx)
        {
            //var top = await banco.Jogadores.Find(FilterDefinition<WafclastJogador>.Empty).Limit(10)
            //    .SortByDescending(x => x.Personagem.Moedas).ToListAsync();
            //StringBuilder str = new StringBuilder();

            //foreach (var item in top)
            //{
            //    var member = await ctx.Client.GetUserAsync(item.Id);
            //    str.AppendLine(Formatter.Bold($"{item.Personagem.Mochila.Moedas}{Emoji.Coins} {member.Mention}"));
            //}
            //DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            //embed.WithDescription(str.ToString());
            //embed.WithTitle("Jogadores mais ricos");

            //await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
