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
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            try
            {
                using (var session = await ModuloBanco.Cliente.StartSessionAsync())
                {
                    BancoSession banco = new BancoSession(session);
                    RPJogador jogador = await banco.GetJogadorAsync(ctx);

                    if (jogador.Personagem.Zona.Monstros == null)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, porque atacar o vento? Explore uma zona!");
                        return;
                    }

                    idEscolhido = idEscolhido.Replace("#", string.Empty);
                    bool converteu = int.TryParse(idEscolhido, out int id);
                    if (!converteu)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, você informou um #ID válido?");
                        return;
                    }
                    RPPersonagem personagem = jogador.Personagem;
                    RPZona zona = personagem.Zona;

                    id = Math.Clamp(id, 0, zona.Monstros.Count - 1);



                    StringBuilder ataquesInimigos = new StringBuilder();
                    bool personagemMorreu = false;
                    while (!personagemMorreu)
                    {
                        foreach (var item in personagem.Zona.Monstros)
                        {
                            item.Acao += item.VelocidadeAtaque;
                            if (item.Acao >= zona.PontosDeAcaoTotal)
                            {

                                item.Acao = 0;
                                zona.Turno++;
                                //Faz calculo para acertar, se errar avisa que desviou do ataque
                                var chanceAcerto = Calculo.Chance(Calculo.ChanceAcertar(item.Precisao, personagem.Evasao.Atual));
                                if (chanceAcerto)
                                {
                                    //Caso contrario faz o de dano.
                                    double danoMonstro = Calculo.CalcularDano(item.Dano, personagem.Armadura.Atual);
                                    personagem.Vida.Diminuir(danoMonstro);
                                    ataquesInimigos.AppendLine($"{item.Nome} causou {danoMonstro.Text()} de dano físico.");
                                    if (personagem.Vida.Atual <= 0)
                                    {
                                        personagemMorreu = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    ataquesInimigos.AppendLine($"{item.Nome} errou o ataque!");
                                }
                            }
                        }
                        personagem.PontosDeAcao += personagem.VelocidadeAtaque.Atual;
                        if (personagem.PontosDeAcao >= zona.PontosDeAcaoTotal)
                        {

                            personagem.PontosDeAcao = 0;
                            zona.Turno++;
                            break;
                        }
                    }
                    DiscordEmbedBuilder embed = new DiscordEmbedBuilder();

                    //Recupera vida/mana por sec se usou
                    //Avisa quantos de vida/mana recuperou o pot

                    //Verifica se acertou, se acertou causa dano
                    var chanceAcertoPersonagem = Calculo.Chance(Calculo.ChanceAcertar(personagem.Precisao.Atual, zona.Monstros[id].Evasao));
                    if (chanceAcertoPersonagem)
                    {
                        double danoPersonagem = personagem.DanoFisico.Sortear;
                        zona.Monstros[id].Vida -= danoPersonagem;
                        embed.AddField("Ataques".Titulo(), $"{ataquesInimigos}\nVocê causou {danoPersonagem.Text()} de dano no {zona.Monstros[id].Nome}!", true);
                        if (zona.Monstros[id].Vida <= 0)
                        {
                            embed.AddField("Mortos".Titulo(), $"{Emoji.CrossBone} {zona.Monstros[id].Nome.Bold()} ️{Emoji.CrossBone}\n" +
                                $"+{zona.Monstros[id].Exp} exp.", true);
                            //Faz calculo de drop
                            //Avisa os drops que caiu
                            zona.SortearItem();
                            int evoluiu = personagem.AddExp(zona.Monstros[id].Exp);
                            if (evoluiu != 0)
                                embed.AddField("Evolução".Titulo(), $"{Emoji.Up} Você evoluiu de nível!");
                            zona.Monstros.Remove(zona.Monstros[id]);
                            int inimigosNovos = zona.NovaOnda(personagem.VelocidadeAtaque.Atual);
                            if (inimigosNovos != 0)
                            {
                                embed.AddField("Nova onda".Titulo(), $"Apareceu {inimigosNovos} monstros!");
                            }
                        }
                    }
                    else
                    {
                        embed.AddField("Ataques".Titulo(), $"{ataquesInimigos}\nVocê errou o ataque!", true);
                    }
                    embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);
                    embed.WithDescription($"Onda {zona.OndaAtual.Bold()}/{zona.OndaTotal.Bold()}.\n" +
                        $"Turno {zona.Turno}\n");





                    if (personagemMorreu)
                    {
                        personagem.Zona = new RPZona();
                        personagem.Vida.Adicionar(double.MaxValue);
                        personagem.Mana.Adicionar(double.MaxValue);
                        embed.Description += "Você morreu!!!".Bold();
                    }

                    await banco.EditJogadorAsync(jogador);
                    await session.CommitTransactionAsync();

                    await ctx.RespondAsync(embed: embed.Build());
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
