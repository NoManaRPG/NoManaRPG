using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoAtacar : BaseCommandModule
    {
        [Command("atacar")]
        public async Task ComandoAtacarAsync(CommandContext ctx, string idEscolhido = "0")
        {
            // Verifica se existe o jogador,
            // Caso não exista avisar no chat e finaliza o metodo.
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;


            // Usar try catch  devido a sessão do Mongo..
            try
            {
                // Inicia uma sessão do Mongo para não ter alteração duplicada.
                using (var session = await ModuloBanco.Cliente.StartSessionAsync())
                {
                    BancoSession banco = new BancoSession(session);
                    RPJogador jogador = await banco.GetJogadorAsync(ctx);
                    RPPersonagem personagem = jogador.Personagem;

                    if (personagem.Zona.Monstros == null)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, porque atacar o vento? Explore uma zona!");
                        return;
                    }

                    // Converte o id informado.
                    bool converteu = int.TryParse(idEscolhido.Replace("#", string.Empty), out int id);
                    if (!converteu)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você informou um #ID válido?");
                        return;
                    }

                    // Limita o id.
                    id = Math.Clamp(id, 0, personagem.Zona.Monstros.Count - 1);

                    // Executa os ataques do inimigo.
                    StringBuilder resumoBatalha = personagem.Zona.CalcAtaquesInimigos(personagem);

                    // Executa os efeitos ativos no personagem.
                    personagem.CalcEfeitos(resumoBatalha);

                    // Exibimos a vida/mana do personagem no começo.
                    DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
                    embed.AddField($"{Emoji.OrbVida} {"Vida".Titulo()}", $"{personagem.Vida.Atual.Text()}/{personagem.Vida.Maximo.Text()}", true);
                    embed.AddField($"{Emoji.OrbMana} {"Mana".Titulo()}", $"{personagem.Mana.Atual.Text()}/{personagem.Mana.Maximo.Text()}", true);

                    var chanceAcertoPersonagem = Calculo.DanoFisicoChanceAcerto(personagem.Precisao.Atual, personagem.Zona.Monstros[id].Evasao);
                    if (chanceAcertoPersonagem)
                    {
                        double danoPersonagem = personagem.DanoFisico.Sortear;
                        personagem.Zona.Monstros[id].Vida -= danoPersonagem;
                        resumoBatalha.AppendLine($"\nVocê causou {danoPersonagem.Text()} de dano no {personagem.Zona.Monstros[id].Nome}!");

                        if (personagem.Zona.Monstros[id].Vida <= 0)
                        {
                            embed.AddField("Mortos".Titulo(), $"{Emoji.CrossBone} {personagem.Zona.Monstros[id].Nome.Bold()} ️{Emoji.CrossBone}\n" +
                                $"+{personagem.Zona.Monstros[id].Exp} exp.");
                            //Recarrega os frascos
                            foreach (var item in personagem.Pocoes)
                            {
                                item.CargasAtual += 1;
                            }
                            //Faz calculo de drop
                            //Avisa os drops que caiu
                            personagem.Zona.SortearItem();

                            int evoluiu = personagem.AddExp(personagem.Zona.Monstros[id].Exp);
                            if (evoluiu != 0)
                                embed.AddField("Evolução".Titulo(), $"{Emoji.Up} Você evoluiu de nível!");

                            personagem.Zona.Monstros.Remove(personagem.Zona.Monstros[id]);
                            int inimigosNovos = personagem.Zona.NovaOnda(personagem.VelocidadeAtaque.Atual);
                            if (inimigosNovos != 0)
                            {
                                embed.AddField("Nova onda".Titulo(), $"Apareceu {inimigosNovos} monstros!");
                            }
                        }
                    }
                    else
                        resumoBatalha.AppendLine($"\nVocê errou o ataque!");

                    embed.WithDescription($"Onda {personagem.Zona.OndaAtual.Bold()}/{personagem.Zona.OndaTotal.Bold()}.\n" +
                        $"Turno {personagem.Zona.Turno}\n");
                    embed.WithColor(DiscordColor.Blue);
                    if (personagem.Vida.Atual <= 0)
                    {
                        personagem.Zona = new RPZona();
                        personagem.Vida.Adicionar(double.MaxValue);
                        personagem.Mana.Adicionar(double.MaxValue);
                        foreach (var item in personagem.Pocoes)
                            item.AddCarga(double.MaxValue);
                        personagem.Efeitos = new List<RPEfeito>();

                        resumoBatalha.AppendLine($"{Emoji.CrossBone} Você morreu!!! {Emoji.CrossBone}".Bold());
                        embed.WithColor(DiscordColor.Red);
                    }

                    embed.AddField("Resumo da batalha".Titulo(), resumoBatalha.ToString());
                    embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);

                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();

                    await ctx.RespondAsync(embed: embed.Build());
                }
            }
            catch (MongoDB.Driver.MongoCommandException)
            {
                await MensagensStrings.ComandoSendoProcessado(ctx);
            }
        }
    }
}
