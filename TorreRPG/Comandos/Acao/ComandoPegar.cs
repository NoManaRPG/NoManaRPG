using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Threading.Tasks;
using TorreRPG.Atributos;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoPegar : BaseCommandModule
    {
        [Command("pegar")]
        [Description("Permite pegar um item que se encontra no chão. `#ID` se encontra no comando `olhar item`.")]
        [ComoUsar("pegar [#ID]")]
        [Exemplo("pegar #1")]
        public async Task ComandoPegarAsync(CommandContext ctx, string stringIndexItem = "0")
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            // Converte o id informado.
            if (!stringIndexItem.TryParseID(out int indexItem))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, o #ID precisa ser numérico!");
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
                        await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente para pegar {item.TipoBaseModificado.Titulo().Bold()}!!");
                }
            }
        }
    }
}
