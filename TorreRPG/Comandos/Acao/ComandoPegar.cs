using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoPegar : BaseCommandModule
    {
        [Command("pegar")]
        public async Task ComandoPegarAsync(CommandContext ctx, string stringIndexItem = "")
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            // Converte o id informado.
            if (!stringIndexItem.TryParseID(out int indexItem))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, o #ID é numérico!");
                return;
            }

            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                if (personagem.Zona.ItensNoChao == null)
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você não tem itens no chão para pegar!");
                    return;
                }

                var item = personagem.Zona.ItensNoChao.ElementAtOrDefault(indexItem);
                if (item != null)
                {
                    if (personagem.Mochila.TryAddItem(item))
                    {
                        personagem.Zona.ItensNoChao.RemoveAt(indexItem);

                        await banco.EditJogadorAsync(jogador);
                        await session.CommitTransactionAsync();

                        await ctx.RespondAsync($"{ctx.User.Mention}, você pegou {item.TipoBaseModificado.Titulo().Bold()}!");
                    }
                    else
                        await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente!");
                }
            }
        }
    }
}
