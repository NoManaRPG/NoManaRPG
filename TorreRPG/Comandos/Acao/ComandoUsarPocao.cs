using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using TorreRPG.Services;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoUsarPocao : BaseCommandModule
    {
        public readonly Banco banco;

        [Command("usar-pocao")]
        [Aliases("usarp")]
        [Description("Permite usar uma poção que foi equipada no cinto.")]
        [ComoUsar("usar-pocao [0 - 4]")]
        [Exemplo("usar-porcao 0")]
        public async Task ComandoUsarPocaoAsync(CommandContext ctx, string stringId = "0")
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.Zona.Monstros.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você só pode usar poções em batalha.");
                return;
            }

            if (personagemNaoModificar.Frascos.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem frascos equipados para usar.");
                return;
            }

            bool usouPocao = false;

            using (var session = await banco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                if (stringId.TryParseID(out int id))
                {
                    id = Math.Clamp(id, 0, personagem.Frascos.Count - 1);

                    if (personagem.Frascos[id].CargasAtual >= personagem.Frascos[id].CargasUso)
                    {
                        switch (personagem.Frascos[id].Classe)
                        {
                            case Enuns.RPClasse.Frasco:
                                usouPocao = true;
                                personagem.Frascos[id].RemoverCarga(personagem.Frascos[id].CargasUso);
                                double duracao = personagem.Frascos[id].Tempo / personagem.VelocidadeAtaque.Modificado;
                                personagem.Efeitos.Add(new RPEfeito(Enuns.RPClasse.Frasco, "Regeneração de vida", duracao, personagem.Frascos[id].Regen / duracao, personagem.VelocidadeAtaque.Modificado));
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
                        await ctx.RespondAsync($"{ctx.User.Mention}, você acabou de usar { personagem.Frascos[id].TipoBaseModificado.Titulo().Bold()}!");
                }
                else
                    await ctx.RespondAsync($"{ctx.User.Mention}, o `#ID` precisa ser numérico. Digite `!equipamentos` para encontrar `#ID`s.");
            }
        }
    }
}
