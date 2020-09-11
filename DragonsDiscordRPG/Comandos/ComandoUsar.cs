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
        public async Task ComandoUsarAsync(CommandContext ctx, int posicao = 0)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            bool usouPocao = false;

            try
            {
                using (var session = await ModuloBanco.Cliente.StartSessionAsync())
                {
                    BancoSession banco = new BancoSession(session);
                    RPJogador jogador = await banco.GetJogadorAsync(ctx);
                    RPPersonagem personagem = jogador.Personagem;

                    if (personagem.Zona.Monstros == null)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você só pode usar poções em batalha.");
                        return;
                    }
                    posicao = Math.Clamp(posicao, 0, 4);

                    if (personagem.Pocoes.Count - 1 < posicao)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, o slot **{posicao}** não tem um frasco equipado!");
                        return;
                    }

                    if (personagem.Pocoes[posicao].CargasAtual >= personagem.Pocoes[posicao].CargasUso)
                    {
                        switch (personagem.Pocoes[posicao].Tipo)
                        {
                            case Enuns.RPTipo.PocaoVida:
                                usouPocao = true;
                                personagem.Pocoes[posicao].RemoverCarga(personagem.Pocoes[posicao].CargasUso);
                                double duracao = personagem.Pocoes[posicao].Tempo / personagem.VelocidadeAtaque.Atual;
                                personagem.Efeitos.Add(new RPEfeito(Enuns.RPTipo.PocaoVida, "Regeneração de vida", duracao, personagem.Pocoes[posicao].LifeRegen / duracao, personagem.VelocidadeAtaque.Atual));
                                break;
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
                        await ctx.RespondAsync($"{ctx.User.Mention}, você acabou de usar { personagem.Pocoes[posicao].Nome.Titulo().Bold()}!");
                }
            }
            catch (MongoDB.Driver.MongoCommandException)
            {
                await MensagensStrings.ComandoSendoProcessado(ctx);
            }
        }

        [Command("usar")]
        public async Task ComandoUsarAsync(CommandContext ctx, string item)
        {
            await ctx.RespondAsync($"{ctx.User.Mention}, você só pode usar poções! De 0 a 4.");
        }
    }
}
