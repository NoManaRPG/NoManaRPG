using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoVoltar : BaseCommandModule
    {
        [Command("voltar")]
        public async Task ComandoVoltarAsync(CommandContext ctx)
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
                        await ctx.RespondAsync($"{ctx.User.Mention}, você precisa eliminar todos os montros para voltar!");
                        return;
                    }

                    bool temMonstros = ModuloBanco.MonstrosNomes.ContainsKey(personagem.Zona.Nivel - 1);
                    if (temMonstros)
                    {

                        inimigos = personagem.Zona.TrocarZona(personagem.VelocidadeAtaque.Atual, personagem.Zona.Nivel - 1);

                        await banco.EditJogadorAsync(jogador);
                        await session.CommitTransactionAsync();
                        await ctx.RespondAsync($"{ctx.User.Mention}, apareceu {inimigos} monstro na sua frente!");
                    }
                    else
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, única coisa que você pode voltar agora é do discord!");
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
