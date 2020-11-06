using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoMochila : BaseCommandModule
    {
        public Banco banco;

        [Command("mochila")]
        [Description("Permite ver os itens que estão na mochila.")]
        [ComoUsar("mochila [PAGINA]")]
        [Exemplo("mochila 1")]
        public async Task ComandoMochilaAsync(CommandContext ctx, int pagina = 0)
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx);
            if (!isJogadorCriado) return;

            StringBuilder str = new StringBuilder();
            str.AppendLine($"**{Emoji.Coins} {sessao.Jogador.Personagem.Mochila.Moedas}**");
            var pag = GetPage(sessao.Jogador.Personagem.Mochila.Itens, pagina, 10);

            for (int i = 0; i < pag.Count; i++)
            {
                var item = pag[i];
                str.Append($"`#{i}` ");
                str.Append($"{item.Nome.Titulo().Bold()} ");
                if (pag[i] is WafclastItemEmpilhavel)
                    str.Append($"*x{((WafclastItemEmpilhavel)pag[i]).Pilha}*");
                str.AppendLine();
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder().Criar(ctx);
            embed.WithTitle($"Mochila nível {sessao.Jogador.Personagem.Mochila.Nivel}.");
            if (string.IsNullOrWhiteSpace(str.ToString()))
                embed.WithDescription("*Parece que sua mochila está vazia, por que não explore um pouco?*");
            else
                embed.WithDescription(str.ToString());
            embed.WithFooter($"Espaço {sessao.Jogador.Personagem.Mochila.EspacoAtual}/{sessao.Jogador.Personagem.Mochila.EspacoMax} | Pagina {pagina}.");
            await ctx.RespondAsync(embed: embed.Build());
        }

        public List<WafclastItem> GetPage(List<WafclastItem> list, int page, int pageSize)
                => list.Skip(page * pageSize).Take(pageSize).ToList();
    }
}
