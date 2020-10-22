using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Entidades;
using TorreRPG.Entidades.Itens;
using TorreRPG.Enuns;
using TorreRPG.Extensoes;
using TorreRPG.Metadata.Itens.MoedasEmpilhaveis;
using TorreRPG.Services;

namespace TorreRPG.Comandos.Acao
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

            if (quantidade <= 0)
            {
                await ctx.RespondAsync("Você precisa informar uma quantidade maior que 0!");
                return;
            }

            if (string.IsNullOrEmpty(stringItem))
            {
                await ctx.RespondAsync("Você precisa informar o nome do item que deseja comprar!");
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
                string itemNome = "";

                switch (stringItem)
                {
                    case "pergaminho de portal":
                    case "portal":
                        bool tem = personagem.Mochila.TryRemoveItemCurrency(RPClasse.PergaminhoSabedoria, out RPBaseItem pergaminhoSabedoria, 3 * quantidade);
                        if (tem)
                        {
                            var item = new MoedasEmpilhaveis().PergaminhoPortal();
                            itemNome = item.TipoBaseModificado;
                            personagem.Mochila.TryAddItem(item);
                            vendeu = true;
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

                    await ctx.RespondAsync($"{ctx.User.Mention}, você acabou de comprar {quantidade.Bold()} {itemNome.Titulo()}!");
                    return;
                }


            }

        }

        [Command("comprar")]
        [Priority(0)]
        public async Task ComandoComprarAsync(CommandContext ctx, string quantidade = "")
        {
            await ctx.RespondAsync("Você precisa informar uma quantidade maior que 0!");
        }
    }
}
