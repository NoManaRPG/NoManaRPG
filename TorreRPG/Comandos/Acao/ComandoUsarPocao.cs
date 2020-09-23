using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoUsarPocao : BaseCommandModule
    {
        [Command("usar-pocao")]
        [Aliases("usarp")]
        [Description("Permite usar uma poção que foi equipada no cinto.")]
        [ComoUsar("usar-pocao [0 - 4]")]
        [Exemplo("usar-porcao 0")]
        public async Task ComandoUsarPocaoAsync(CommandContext ctx, int stringPosicao = 0)
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
                if(personagem.Frascos.Count == 0)
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você não tem frascos equipados para usar.");
                    return;
                }
                stringPosicao = Math.Clamp(stringPosicao, 0, personagem.Frascos.Count - 1);

                if (personagem.Frascos[stringPosicao].CargasAtual >= personagem.Frascos[stringPosicao].CargasUso)
                {
                    switch (personagem.Frascos[stringPosicao].Classe)
                    {
                        case Enuns.RPClasse.Frasco:
                            usouPocao = true;
                            personagem.Frascos[stringPosicao].RemoverCarga(personagem.Frascos[stringPosicao].CargasUso);
                            double duracao = personagem.Frascos[stringPosicao].Tempo / personagem.VelocidadeAtaque.Modificado;
                            personagem.Efeitos.Add(new RPEfeito(Enuns.RPClasse.Frasco, "Regeneração de vida", duracao, personagem.Frascos[stringPosicao].Regen / duracao, personagem.VelocidadeAtaque.Modificado));
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
                    await ctx.RespondAsync($"{ctx.User.Mention}, você acabou de usar { personagem.Frascos[stringPosicao].TipoBaseModificado.Titulo().Bold()}!");
            }
        }
    }
}
