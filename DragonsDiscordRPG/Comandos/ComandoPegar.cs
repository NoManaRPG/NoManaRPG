using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Enuns;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoPegar : BaseCommandModule
    {
        [Command("pegar")]
        public async Task ComandoPegarAsync(CommandContext ctx, string idEscolhido)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            try
            {
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

                    // Converte o id informado.
                    bool converteu = int.TryParse(idEscolhido.Replace("#", string.Empty), out int id);
                    if (!converteu)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você informou um #ID válido?");
                        return;
                    }

                    var item = personagem.Zona.ItensNoChao.ElementAtOrDefault(id);
                    if (item != null)
                    {
                        if (personagem.Mochila.TryAddItem(item))
                        {
                            personagem.Zona.ItensNoChao.RemoveAt(id);

                            await banco.EditJogadorAsync(jogador);
                            await session.CommitTransactionAsync();

                            await ctx.RespondAsync($"{ctx.User.Mention}, você pegou {item.Nome.Titulo().Bold()}!");
                        }
                        else
                            await ctx.RespondAsync($"{ctx.User.Mention}, você não tem espaço o suficiente!");
                    }
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
