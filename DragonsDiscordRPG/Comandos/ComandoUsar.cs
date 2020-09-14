using DragonsDiscordRPG.Atributos;
using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoUsar : BaseCommandModule
    {
        [Command("usar")]
        [Description("Permite usar uma poção que foi equipada no cinto.")]
        [ComoUsar("usar [0 - 4]")]
        [Exemplo("usar 0")]
        public async Task ComandoUsarAsync(CommandContext ctx, int stringPosicao = 0)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            bool usouPocao = false;

            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                if (personagem.Zona.Monstros.Count == 0)
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você só pode usar poções em batalha.");
                    return;
                }
                stringPosicao = Math.Clamp(stringPosicao, 0, personagem.Pocoes.Count - 1);

                if (personagem.Pocoes[stringPosicao].CargasAtual >= personagem.Pocoes[stringPosicao].CargasUso)
                {
                    switch (personagem.Pocoes[stringPosicao].Tipo)
                    {
                        case Enuns.RPItemTipo.PocaoVida:
                            usouPocao = true;
                            personagem.Pocoes[stringPosicao].RemoverCarga(personagem.Pocoes[stringPosicao].CargasUso);
                            double duracao = personagem.Pocoes[stringPosicao].Tempo / personagem.VelocidadeAtaque.Atual;
                            personagem.Efeitos.Add(new RPEfeito(Enuns.RPItemTipo.PocaoVida, "Regeneração de vida", duracao, personagem.Pocoes[stringPosicao].LifeRegen / duracao, personagem.VelocidadeAtaque.Atual));
                            break;
                        default:
                            await ctx.RespondAsync("Frasco não usavel ainda!");
                            return;
                    }
                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, o frasco não tem cargas o suficiente!");
                    return;
                }

                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                if (usouPocao)
                    await ctx.RespondAsync($"{ctx.User.Mention}, você acabou de usar { personagem.Pocoes[stringPosicao].Nome.Titulo().Bold()}!");
            }
        }

        [Command("usar")]
        public async Task ComandoUsarAsync(CommandContext ctx, string item)
        {
            await ctx.RespondAsync($"{ctx.User.Mention}, as posições são de 0 a 4!");
        }
    }
}
