using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Enuns;
using WafclastRPG.Game.Extensoes;
using WafclastRPG.Game.Metadata.Itens.MoedasEmpilhaveis;
using WafclastRPG.Game.Services;

namespace WafclastRPG.Game.Comandos.Acao
{
    public class ComandoComprar : BaseCommandModule
    {
        public Banco banco;

        [Command("comprar")]
        [Priority(1)]
        public async Task ComandoComprarAsync(CommandContext ctx, int quantidade, [RemainingText] string stringItem)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.Zona.Nivel != 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você somente pode comprar itens fora da torre!");
                return;
            }

            if (quantidade <= 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar uma quantidade maior que 0!");
                return;
            }

            if (string.IsNullOrEmpty(stringItem))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar o nome do item que deseja comprar!");
                return;
            }

            stringItem = StringExtension.RemoverAcentos(stringItem.ToLower());

            // Inicia uma sessão do Mongo para não ter alteração duplicada.
            using (var session = await banco.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;
                bool vendeu = false;
                RPBaseItem item = null;

                switch (stringItem)
                {
                    case "pergaminho de portal":
                    case "portal":
                        bool tem = personagem.Mochila.TryRemoveItemCurrency(RPClasse.PergaminhoSabedoria, out RPBaseItem pergaminhoSabedoria, 3 * quantidade);
                        if (tem)
                        {
                            item = new MoedasEmpilhaveis().PergaminhoPortal();
                            for (int i = 0; i < quantidade; i++)
                                vendeu = personagem.Mochila.TryAddItem(item);
                        }
                        else
                        {
                            await ctx.RespondAsync($"{ctx.User.Mention}, você não tem {(3 * quantidade).Bold()} {"Pergaminho de Sabedoria".Titulo()} para comprar este item!");
                            return;
                        }
                        break;
                }

                if (vendeu)
                {
                    // Salvamos.
                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();

                    await ctx.RespondAsync($"{ctx.User.Mention}, você acabou de comprar {quantidade.Bold()} {item.TipoBaseModificado.Titulo()}!");
                }
                else
                    await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente na mochila para comprar este item!");
            }
        }

        [Command("comprar")]
        [Priority(0)]
        public async Task ComandoComprarAsync(CommandContext ctx, string quantidade = "")
        {
            await ctx.RespondAsync($"{ctx.User.Mention}, você precisa informar uma quantidade maior que 0!");
        }
    }
}
