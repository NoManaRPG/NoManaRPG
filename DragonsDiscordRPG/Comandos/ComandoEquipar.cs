using DragonsDiscordRPG.Atributos;
using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    [Group("equipar")]
    public class ComandoEquipar : BaseCommandModule
    {
        [GroupCommand()]
        public async Task ComandoEquiparAsync(CommandContext ctx)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription("Use `!equipar pocao` para equipar poções no cinto.\n" +
                "Use `!equipar item` para equipar itens.");
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("pocao")]
        [Aliases("p")]
        [Description("Permite equipar uma poção no cinto para uso futuro.\nO #ID se contra na mochila!")]
        [ComoUsar("equipar pocao [#ID Item] [0-4]")]
        [Exemplo("equipar pocao #24 0")]
        public async Task ComandoEquiparAsync(CommandContext ctx, string stringIndexItem = "", string stringPosicao = "0")
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            if (!stringIndexItem.TryParseID(out int indexItem))
            {
                await ctx.ExecutarAjudaAsync();
                return;
            }

            if (!stringPosicao.TryParseID(out int posicao))
            {
                await ctx.ExecutarAjudaAsync();
                return;
            }
            posicao = Math.Clamp(posicao, 0, 4);

            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                // Limita o id.

                bool equipou = false;
                // Pega o item
                var pocaoEscolhida = personagem.Mochila.TryRemoveItem(indexItem);
                if (pocaoEscolhida != null)
                {
                    if (pocaoEscolhida.Tipo == Enuns.RPItemTipo.PocaoVida)
                    {
                        // Tem poção equipada na posição escolhida?
                        var pocaoEquipada = personagem.Pocoes.ElementAtOrDefault(posicao);
                        if (pocaoEquipada != null)
                        {
                            // Tenta guardar essa poção 
                            pocaoEquipada.Quantidade = 1;
                            pocaoEquipada.CargasAtual = 0;
                            if (personagem.Mochila.TryAddItem(pocaoEquipada))
                            {
                                //Equipa a poção nova na posição
                                personagem.Pocoes[posicao] = pocaoEscolhida;
                                equipou = true;
                            }
                            else
                            {
                                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente para guardar a poção equipada no cinto!");
                                return;
                            }
                        }
                        else
                        {
                            //Equipa a poção nova na posição
                            personagem.Pocoes.Add(pocaoEscolhida);
                            equipou = true;
                        }
                    }
                    else
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, este item não é uma poção de vida ou de mana para voce equipar no cinto!");
                        return;
                    }
                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, #ID não encontrado!");
                    return;
                }

                if (personagem.Zona.Nivel == 0)
                    pocaoEscolhida.CargasAtual = pocaoEscolhida.CargasMax;

                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                if (equipou)
                    await ctx.RespondAsync($"{ctx.User.Mention}, poção {pocaoEscolhida.Nome.Titulo().Bold()} equipada no slot {posicao}!");
            }
        }
    }
}
