using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Linq;
using System.Threading.Tasks;
using WafclastRPG.Game.Atributos;
using WafclastRPG.Game.Services;
using System;

namespace WafclastRPG.Game.Comandos.Acao
{
    public class ComandoPegar : BaseCommandModule
    {
        public Banco banco;

        [Command("pegar")]
        [Description("Permite pegar um item que se encontra no chão. `#ID` se encontra no comando `olhar item`.")]
        [ComoUsar("pegar [#ID]")]
        [Exemplo("pegar #1")]
        public async Task ComandoPegarAsync(CommandContext ctx, string stringIndexItem = "0")
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.IsPortalAberto)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não pode usar este comando com o portal aberto!");
                return;
            }

            // Converte o id informado.
            if (!stringIndexItem.TryParseID(out int indexItem))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, o #ID precisa ser numérico!");
                return;
            }

            if (personagemNaoModificar.Zona.ItensNoChao == null || personagemNaoModificar.Zona.ItensNoChao.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem itens no chão para pegar!");
                return;
            }

            using (var session = await banco.Client.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

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
