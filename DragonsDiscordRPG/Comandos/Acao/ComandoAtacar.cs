using DragonsDiscordRPG.Atributos;
using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos.Acao
{
    public class ComandoAtacar : BaseCommandModule
    {
        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite atacar um monstro a sua frente.")]
        [ComoUsar("atacar [#ID]")]
        [ComoUsar("atacar")]
        [Exemplo("atacar #1")]
        public async Task ComandoAtacarAsync(CommandContext ctx, string stringIndexAlvo = "#0")
        {
            // Verifica se existe o jogador,
            // Caso não exista avisar no chat e finaliza o metodo.
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            // Inicia uma sessão do Mongo para não ter alteração duplicada.
            using (var session = await ModuloBanco.Cliente.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                if (personagem.Zona.Monstros.Count == 0)
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você precisa explorar um andar na torre para poder estar atacando monstros!");
                    return;
                }

                // Converte o id informado.
                if (!stringIndexAlvo.TryParseID(out int indexAlvo))
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, o #ID é numérico!");
                    return;
                }

                // Limita o id.
                indexAlvo = Math.Clamp(indexAlvo, 0, personagem.Zona.Monstros.Count - 1);

                StringBuilder resumoBatalha = new StringBuilder();

                // Executa os efeitos ativos no personagem.
                personagem.CalcEfeitos(resumoBatalha);

                // Executa os ataques do inimigo.
                personagem.Zona.CalcAtaquesInimigos(personagem, resumoBatalha);

                // Exibimos a vida/mana do personagem no começo da mensagem.
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                embed.AddField($"{Emoji.OrbVida} {"Vida".Titulo()}", $"{personagem.Vida.Atual.Text()}/{personagem.Vida.Maximo.Text()}", true);
                embed.AddField($"{Emoji.OrbMana} {"Mana".Titulo()}", $"{personagem.Mana.Atual.Text()}/{personagem.Mana.Maximo.Text()}", true);

                // Verifica se o personagem vai acertar o monstro
                var chanceAcertoPersonagem = Calculo.DanoFisicoChanceAcerto(personagem.Precisao.Atual, personagem.Zona.Monstros[indexAlvo].Evasao);
                if (chanceAcertoPersonagem)
                {
                    // Randomizamos um dano médio com base no minimo e max da arma equipada.
                    double danoPersonagem = personagem.DanoFisico.Sortear;
                    personagem.Zona.Monstros[indexAlvo].Vida -= danoPersonagem;
                    resumoBatalha.AppendLine($"\nVocê causou {danoPersonagem.Text()} de dano no {personagem.Zona.Monstros[indexAlvo].Nome}!");

                    // Se o monstro morrer.
                    if (personagem.Zona.Monstros[indexAlvo].Vida <= 0)
                    {
                        double expGanha = Calculo.CalcularEfetividadeXP(personagem.Nivel.Atual, personagem.Zona.Monstros[indexAlvo].Nivel) * personagem.Zona.Monstros[indexAlvo].Exp;
                        embed.AddField("Mortos".Titulo(), $"{Emoji.CrossBone} {personagem.Zona.Monstros[indexAlvo].Nome.Bold()} ️{Emoji.CrossBone}\n" +
                            $"+{expGanha.Text()} exp.");

                        int evoluiu = personagem.AddExp(expGanha);
                        if (evoluiu != 0)
                            embed.AddField("Evolução".Titulo(), $"{Emoji.Up} Você evoluiu de nível!");

                        // Recarrega os frascos
                        foreach (var item in personagem.Pocoes)
                            item.AddCarga(1);

                        // Dropa itens
                        if (personagem.Zona.SortearItem(personagem.Zona.Monstros[indexAlvo], personagem.ChanceDrop, out int drops))
                            embed.AddField("Drops".Titulo(), $"Caiu {drops} itens!");

                        if (personagem.Zona.NovaOnda(personagem.VelocidadeAtaque.Atual, out int inimigosNovos))
                            embed.AddField("Nova onda".Titulo(), $"Apareceu {inimigosNovos} monstros!");
                    }
                }
                else // Caso ele erre o ataque
                    resumoBatalha.AppendLine($"\nVocê errou o ataque!");

                // Informações complementares
                embed.WithDescription($"Onda {personagem.Zona.OndaAtual.Bold()}/{personagem.Zona.OndaTotal.Bold()}.\n" +
                 $"Turno {personagem.Zona.Turno}\n" +
                  $"Inimigos { personagem.Zona.Monstros.Count}");

                embed.WithColor(DiscordColor.Blue);
                // Se o personagem morrer, reseta ele.
                if (personagem.Vida.Atual <= 0)
                {
                    personagem.Resetar();

                    resumoBatalha.AppendLine($"{Emoji.CrossBone} Você morreu!!! {Emoji.CrossBone}".Bold());
                    embed.WithColor(DiscordColor.Red);
                }

                // Exibe o resumo da batalha.. Regens e ataques.
                embed.AddField("Resumo da batalha".Titulo(), resumoBatalha.ToString());
                embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);

                // Salvamos.
                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                await ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());
            }
        }
    }
}
