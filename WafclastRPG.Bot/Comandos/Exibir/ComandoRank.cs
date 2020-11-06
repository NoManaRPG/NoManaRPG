using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoRank : BaseCommandModule
    {
        public Banco banco;

        [Command("top")]
        [Description("Exibe os 10 personagens mais ricos")]
        [ComoUsar("top")]
        [Cooldown(1, 30, CooldownBucketType.User)]
        public async Task ComandoMochilaAsync(CommandContext ctx)
        {
            var top = await banco.Jogadores.Find(FilterDefinition<WafclastJogador>.Empty).Limit(10)
                .SortByDescending(x => x.Personagem.Mochila.Moedas).ToListAsync();
            StringBuilder str = new StringBuilder();

            int pos = 1;
            foreach (var item in top)
            {
                try
                {
                    var member = await ctx.Guild.GetMemberAsync(item.Id);
                    str.AppendLine($"{pos}. {member.Mention} - {Emoji.Coins} {item.Personagem.Mochila.Moedas}".Bold());
                }
                catch (Exception)
                {
                    str.AppendLine($"{pos}. {item.Id} - {Emoji.Coins} {item.Personagem.Mochila.Moedas}".Bold());
                }
                pos++;
            }
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription(str.ToString());

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
