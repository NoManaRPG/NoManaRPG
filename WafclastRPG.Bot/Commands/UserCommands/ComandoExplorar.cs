using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Entidades.Proficiencias;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Bot.Comandos.Acao
{
    public class ComandoExplorar : BaseCommandModule
    {
        public Banco banco;
        public BotMathematics rdg;

        [Command("explorar")]
        [Aliases("ex")]
        [Description("Você explora uma região atacando monstros que vê no caminho.")]
        [Example("explorar 2", "Você ataca um monstro por 1.2 segundos .")]
        [Example("explorar", "Você ataca um monstro por 2.4 segundos")]
        [Usage("explorar [ ticks ]")]
        [Cooldown(1, 6, CooldownBucketType.User)]
        public async Task ComandoExplorarAsync(CommandContext ctx, string stringTicks = "4")
        {
            /*
             * Monstro e Jogador batalham entre sí por x(ticks) tempo.
             * Caso o monstro já tenha morrido, faz aparecer outro.
             * Caso o jogador morra, ele perde todos os itens da mochila e equipados, voltando assim para a região 0.
             * Caso o monstro morra, ele continua no chão para ser usado no comando saquear.
             */

            // Trava o jogador de inicializar outro comando.
            using (await banco.LockAsync(ctx.User.Id))
            {
                if (!int.TryParse(stringTicks, out int ticks))
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, ticks informado é inválido.");
                    return;
                }
                // Limitamos para não ultrapassar o limite de caracteres do Discord.
                ticks = Math.Clamp(ticks, 2, 20);

                var jogador = await banco.GetJogadorAsync(ctx);
                var per = jogador.Personagem;

                // Sempre precisa ter inimigos para batalhar.
                if (per.InimigoMonstro == null || per.InimigoMonstro.Vida <= 0)
                {
                    var reg = await jogador.GetRegiaoAsync();
                    per.InimigoMonstro = rdg.Sortear(reg.Monstros);
                }

                var batalha = new StringBuilder();
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder().Inicializar(ctx.User);
                embed.WithDescription($"Ataque durou {Emoji.Relogio} **{(ticks * 0.6):N0}s**");

                // Ataques
                var habAtaque = per.GetHabilidade(ProficienciaType.Ataque) as WafclastProficienciaAtaque;
                var habForca = per.GetHabilidade(ProficienciaType.Forca) as WafclastProficienciaForca;
                WafclastItemArma arma1 = null;
                WafclastItemArma arma2 = null;
                var chance = habAtaque.ChanceAcerto(habAtaque.Precisao, per.InimigoMonstro.GetDefesa());
                bool temArma = false;
                while (ticks > 0)
                {
                    if (per.TryGetEquipamento(EquipamentoType.PrimeiraMao, out var item))
                    {
                        temArma = true;
                        arma1 = item as WafclastItemArma;
                        if (Atacar(chance, arma1, habForca, habAtaque, per, batalha))
                            break;
                    }

                    if (per.TryGetEquipamento(EquipamentoType.SegundaMao, out item))
                    {
                        if (item is WafclastItemArma)
                        {
                            temArma = true;
                            arma2 = item as WafclastItemArma;
                            if (Atacar(chance, arma2, habForca, habAtaque, per, batalha))
                                break;
                        }
                    }
                    if (!temArma)
                    {
                        await ctx.RespondAsync($"{ctx.User.Mention}, é necessário estar equipado com uma arma para explorar.");
                        return;
                    }

                    per.InimigoMonstro.AtaqueVelocidade++;
                    if (per.InimigoMonstro.AtaqueVelocidade == per.InimigoMonstro.AtaqueVelocidadeMax)
                    {
                        per.InimigoMonstro.AtaqueVelocidade = 0;
                        var habDefesa = per.GetHabilidade(ProficienciaType.Defesa) as WafclastProficienciaDefesa;

                        chance = habAtaque.ChanceAcerto(per.InimigoMonstro.GetPrecisao(), habDefesa.Defesa);
                        if (rdg.Chance(chance))
                        {
                            var dano = rdg.Sortear(1, per.InimigoMonstro.DanoMax);
                            if (per.ReceberDano(dano))
                            {
                                batalha.AppendLine($"{Emoji.CrossBone} **Você morreu!** {Emoji.CrossBone}");
                                batalha.AppendLine($"{Emoji.CrossBone} **Você perdeu seus itens!** {Emoji.CrossBone}");
                                per.Morrer();
                                arma1.AtaqueVelocidade = 0;
                                if (arma2 is WafclastItemArma)
                                    arma2.AtaqueVelocidade = 0;
                                embed.WithColor(DiscordColor.Red);
                                embed.WithImageUrl("https://cdn.discordapp.com/attachments/758139402219159562/769397084284649472/kisspng-headstone-drawing-royalty-free-clip-art-5afd7eb3d84cb3.625146071526562483886.png");
                                await ctx.RespondAsync(embed: embed.Build());
                                await jogador.Salvar();
                                return;
                            }
                            else
                            {
                                batalha.AppendLine($"{Emoji.Mago} {ctx.User.Mention} recebeu {Emoji.Adaga} **{dano}** por {per.InimigoMonstro.Nome}!");
                                habDefesa.AddExperience(dano * 0.133);
                            }
                        }
                        batalha.AppendLine($"{Emoji.Mago} **{ctx.User.Mention} defendeu!** {Emoji.Escudo}");
                    }
                    ticks--;
                }


                batalha.AppendLine(Emoji.Vazio);
                embed.AddField("Resumo".Titulo(), "\n" + batalha.ToString());
                embed.WithDescription($"Inimigo {per.InimigoMonstro.Nome}!");

                var cont = per.GetHabilidade(ProficienciaType.Constituicao) as WafclastProficienciaConstituicao;
                var perVida = (double)cont.Vida / cont.CalcularVida();
                var inimVida = (double)per.InimigoMonstro.Vida / per.InimigoMonstro.VidaMax;
                embed.AddField(Formatter.Underline(ctx.User.Username), $"{Emoji.GerarVidaEmoji(perVida)} {cont.Vida}", true);
                embed.AddField(Formatter.Underline(per.InimigoMonstro.Nome), $"{Emoji.GerarVidaEmoji(inimVida)} {per.InimigoMonstro.Vida}", true);
                embed.WithColor(DiscordColor.Purple);
                embed.WithThumbnail("https://cdn.discordapp.com/attachments/758139402219159562/758425473541341214/sword-and-shield-icon-17.png");

                await ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());
                await jogador.Salvar();
            }
        }

        public bool Atacar(double chance, WafclastItemArma arma, WafclastProficienciaForca habForca, WafclastProficienciaAtaque habAtaque, WafclastPersonagem per, StringBuilder batalha)
        {
            arma.AtaqueVelocidade++;
            if (arma.AtaqueVelocidade >= arma.AtaqueVelocidadeMax)
            {
                arma.AtaqueVelocidade = 0;
                if (rdg.Chance(chance))
                {
                    var dano = rdg.Sortear(1, habForca.Dano);
                    batalha.AppendLine($"{Emoji.Demonio} **{per.InimigoMonstro.Nome}** recebeu **{Emoji.Adaga} {dano}** por {arma.Nome}!");
                    if (per.InimigoMonstro.ReceberDano(dano))
                    {
                        batalha.AppendLine($"{Emoji.CrossBone} {per.InimigoMonstro.Nome} {Emoji.CrossBone}");
                        habForca.AddExperience(per.InimigoMonstro.VidaMax * 0.133);
                        habAtaque.AddExperience(per.InimigoMonstro.VidaMax * 0.133);
                        return true;
                    }
                }
                else
                    batalha.AppendLine($"{Emoji.Demonio} **{per.InimigoMonstro.Nome} defendeu!** {Emoji.Escudo}");
            }
            return false;
        }
    }
}
