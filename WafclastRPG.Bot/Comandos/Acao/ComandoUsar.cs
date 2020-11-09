using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoUsar : BaseCommandModule
    {
        public Banco banco;

        [Command("usar")]
        [Description("Permite usar um item.")]
        [ComoUsar("usar [#ID]")]
        [Exemplo("usar #1")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task ComandoVenderAsync(CommandContext ctx, string stringId = "", string stringQuantidade = "1")
        {
            // Verifica se existe o jogador e faz o jogador esperar antes de começar outro comando
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx, true);
            if (!isJogadorCriado) return;

            var personagem = sessao.Jogador.Personagem;

            if (!stringId.TryParseID(out var index))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar um `#ID` válido!");
                sessao.Soltar();
                return;
            }

            if (!int.TryParse(stringQuantidade, out var quantidade))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar uma `quantidade` válida!");
                sessao.Soltar();
                return;
            }

            if (quantidade <= 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não pode usar {quantidade} item!");
                sessao.Soltar();
                return;
            }
            var resposta = personagem.Mochila.TryRemoveItem(index, out var item, quantidade);
            switch (resposta)
            {
                case WafclastMochila.MochilaResposta.NaoEncontrado:
                    sessao.Soltar();
                    await ctx.RespondAsync($"{ctx.User.Mention}, `#ID` não foi encontrado na `mochila`!");
                    return;
                case WafclastMochila.MochilaResposta.QuantiaInvalida:
                    sessao.Soltar();
                    await ctx.RespondAsync($"{ctx.User.Mention}, você não tem {quantidade}!");
                    return;
            }

            var str = new StringBuilder();
            switch (item)
            {
                #region Frasco
                case WafclastItemFrasco wif:
                    if (wif.Tipo.HasFlag(FrascoTipo.Vida) & wif.Tipo.HasFlag(FrascoTipo.Mana))
                    {
                    }
                    else if (wif.Tipo.HasFlag(FrascoTipo.Vida))
                    {
                        personagem.Vida.Incrementar(wif.VidaRestaura * wif.Pilha);
                        str.AppendLine($"{ctx.User.Mention}, você usou **{wif.Pilha}** {wif.Nome.Titulo()}! :sparkling_heart:+**{wif.VidaRestaura * wif.Pilha}**. ");
                    }
                    else
                    {

                    }
                    break;
                #endregion
                #region Comida
                case WafclastItemComida wic:
                    personagem.Fome.Incrementar(wic.FomeRestaura * wic.Pilha);
                    str.AppendLine($"{ctx.User.Mention}, você comeu **{wic.Pilha}** {wic.Nome.Titulo()}! {Emoji.Fome}**+{wic.FomeRestaura * wic.Pilha}**.");
                    break;
                #endregion
                #region Bebida
                case WafclastItemBebida wib:
                    personagem.Sede.Incrementar(wib.SedeRestaura * wib.Pilha);
                    str.AppendLine($"{ctx.User.Mention}, você bebeu **{wib.Pilha}** {wib.Nome.Titulo()}! :cup_with_straw:**+{wib.SedeRestaura * wib.Pilha}**.");
                    break;
                #endregion
                default:
                    await ctx.RespondAsync($"{ctx.User.Mention}, este item não é usável!");
                    sessao.Soltar();
                    return;
            }

            await sessao.Salvar();
            await ctx.RespondAsync(str.ToString());
        }
    }
}
