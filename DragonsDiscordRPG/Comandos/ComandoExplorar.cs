using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoExplorar : BaseCommandModule
    {
        [Command("explorar")]
        public async Task ComandoExplorarAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;
            try
            {
                int inimigos = 0;
                using (var session = await ModuloBanco.Cliente.StartSessionAsync())
                {
                    BancoSession banco = new BancoSession(session);
                    RPJogador jogador = await banco.GetJogadorAsync(ctx);
                    RPPersonagem personagem = jogador.Personagem;

                    if (personagem.Zona.Monstros != null)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você precisa eliminar todos os montros para explorar!");
                        return;
                    }

                    if(personagem.Zona.Nivel == 0)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você somente pode explorar os níveis inferiores da torre!");
                        return;
                    }

                        inimigos = personagem.Zona.TrocarZona(personagem.VelocidadeAtaque.Atual, personagem.Zona.Nivel);

                        await banco.EditJogadorAsync(jogador);
                        await session.CommitTransactionAsync();
                        await ctx.RespondAsync($"{ctx.User.Mention}, apareceu {inimigos} monstro na sua frente!");
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
