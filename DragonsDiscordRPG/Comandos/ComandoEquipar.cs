using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ComandoEquiparAsync(CommandContext ctx, string idItem = "", string idPosicao = "")
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            bool converteu = int.TryParse(idItem.Replace("#", string.Empty), out int id);
            if (!converteu)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você informou um #ID válido?");
                return;
            }

            converteu = int.TryParse(idPosicao.Replace("#", string.Empty), out int posicao);
            if (!converteu)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você informou uma posição válida?");
                return;
            }

            try
            {
                using (var session = await ModuloBanco.Cliente.StartSessionAsync())
                {
                    BancoSession banco = new BancoSession(session);
                    RPJogador jogador = await banco.GetJogadorAsync(ctx);
                    RPPersonagem personagem = jogador.Personagem;

                    // Limita o id.
                    posicao = Math.Clamp(posicao, 0, 4);

                    bool equipou = false;
                    // Pega o item
                    var pocaoEscolhida = personagem.Mochila.TryRemoveItem(id);
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
            catch (Exception ex)
            {
                await MensagensStrings.ComandoSendoProcessado(ctx);
                throw ex;
            }
        }
    }
}
