using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;
using TorreRPG.Atributos;
using TorreRPG.Entidades.Itens.Currency;
using TorreRPG.Services;
using System;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoExaminar : BaseCommandModule
    {
        public Banco banco { private get; set; }

        [Command("examinar")]
        [Aliases("ex")]
        [Description("Permite examinar um item.\n`#ID` se contra na mochila.")]
        [ComoUsar("examinar [#ID]")]
        [Exemplo("examinar #1")]
        public async Task ComandoExaminarAsync(CommandContext ctx, string idEscolhido = "0")
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            RPJogador jogador = await banco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            if (personagem.Mochila.Itens.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa de itens na mochila para examinar!");
                return;
            }

            // Converte o id informado.
            if (idEscolhido.TryParseID(out int id))
            {
                var item = personagem.Mochila.Itens.ElementAtOrDefault(id);
                if (item != null)
                {
                    DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                    ItemDescricao(embed, item);
                    embed.WithAuthor($"{ctx.User.Username} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);
                    embed.WithTitle($"`#{id}` {item.TipoBaseModificado.Titulo().Bold()}");
                    embed.WithColor(DiscordColor.Yellow);
                    await ctx.RespondAsync(embed: embed.Build());
                }
                else
                    await ctx.RespondAsync($"{ctx.User.Mention}, `#ID` não encontrado!");
            }
            else
                await ctx.RespondAsync($"{ctx.User.Mention}, o `#ID` precisa ser numérico. Digite `!mochila` para encontrar `#ID`s.");
        }
        public static void ItemDescricao(DiscordEmbedBuilder embed, RPBaseItem item)
        {
            switch (item)
            {
                case RPFrascoVida frascoVida:
                    embed.WithDescription(frascoVida.Descricao());
                    break;
                case RPBaseItemArma arma:
                    embed.WithDescription(arma.Descricao());
                    break;
                case RPCurrencyPergaminho pergaminho:
                    embed.WithDescription(pergaminho.Descricao());
                    break;
            }
        }
    }
}
