using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;
using TorreRPG.Atributos;
using System.Text;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoChao : BaseCommandModule
    {
        [Command("chao")]
        [Aliases("drop")]
        [Description("Permite examinar um item.\n`#ID` se contra no chão.")]
        [ComoUsar("chao [#ID]")]
        [Exemplo("chao #1")]
        public async Task ComandoChaoAsync(CommandContext ctx, string idEscolhido = "")
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            if (personagem.Zona.ItensNoChao.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa de itens no chão para olhar! Elimine alguns monstros!");
                return;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithColor(DiscordColor.Yellow);
            if (string.IsNullOrWhiteSpace(idEscolhido))
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < personagem.Zona.ItensNoChao.Count; i++)
                {
                    var item = personagem.Zona.ItensNoChao[i];
                    str.AppendLine($"`#{i}` {item.TipoBaseModificado.Titulo().Bold()} ");
                }
                embed.WithDescription("Você está olhando para os itens no chão! Digite `!pegar` para guarda-los na mochila!\n" + str.ToString());
                await ctx.RespondAsync(embed: embed.Build());
                return;
            }

            // Converte o id informado.
            if (idEscolhido.TryParseID(out int id))
            {
                var item = personagem.Zona.ItensNoChao.ElementAtOrDefault(id);
                if (item != null)
                {
                    ComandoExaminar.ItemDescricao(embed, item);
                    embed.WithAuthor($"{ctx.User.Username} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);
                    embed.WithTitle($"`#{id}` {item.TipoBaseModificado.Titulo().Bold()}");
                    await ctx.RespondAsync(embed: embed.Build());
                }
                else
                    await ctx.RespondAsync($"{ctx.User.Mention}, `#ID` não encontrado!");
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, o `#ID` precisa ser numérico. Digite `!chao` para encontrar `#ID`s.");
        }
    }
}
