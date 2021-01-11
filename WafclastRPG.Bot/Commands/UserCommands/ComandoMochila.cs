using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoMochila : BaseCommandModule
    {
        public Banco banco;

        [Command("mochila")]
        [Description("Permite ver os itens que estão na mochila.")]
        [Example("mochila 1", "Mostra 8 itens apartir do indice 0")]
        [Usage("mochila [ pagina ]")]
        public async Task ComandoMochilaAsync(CommandContext ctx, string stringPagina = "0")
        {
            using (await banco.LockAsync(ctx.User.Id))
            {
                var jogador = await banco.GetJogadorAsync(ctx);
                var per = jogador.Personagem;

                int.TryParse(stringPagina, out var pagina);

                StringBuilder str = new StringBuilder();
                str.AppendLine($"**{per.PortaNiqueis}** {Emoji.Coins}");
                var pag = GetPage(per.Mochila.Itens, pagina, 8);

                for (int i = 0; i < pag.Count; i++)
                {
                    var item = await banco.GetItemAsync(pag[i].ItemId);
                    str.Append($"`#{i}` ");
                    str.AppendLine($"{item.Nome} x{pag[i].Quantidade}");
                }

                DiscordEmbedBuilder embed = new DiscordEmbedBuilder().Inicializar(ctx);
                embed.WithDescription(str.ToString());
                embed.WithFooter($"Espaço {per.Mochila.EspacoAtual}/{per.Mochila.EspacoMax} | Pagina {pagina}.");
                await ctx.RespondAsync(embed: embed.Build());
            }
        }

        public List<WafclastMochila.Item> GetPage(List<WafclastMochila.Item> list, int page, int pageSize)
                => list.Skip(page * pageSize).Take(pageSize).ToList();
    }
}
