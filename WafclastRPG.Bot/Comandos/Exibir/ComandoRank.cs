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
        [Cooldown(1, 60, CooldownBucketType.User)]
        public async Task ComandoMochilaAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx);
            if (!isJogadorCriado) return;

            var top = await banco.Jogadores.Find(FilterDefinition<WafclastJogador>.Empty).Limit(10)
                .SortByDescending(x => x.Personagem.Mochila.Moedas).ToListAsync();
            StringBuilder str = new StringBuilder();

            int pos = 1;
            foreach (var item in top)
            {
                var g = await ctx.Client.GetUserAsync(item.Id);
                str.AppendLine($"{pos}. {g.Mention} - :coin: {item.Personagem.Mochila.Moedas}".Bold());
                pos++;
            }
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription(str.ToString());

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
