using TorreRPG.Atributos;
using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Services;

namespace TorreRPG.Comandos.Acao
{
    public class ComandoAtacar : BaseCommandModule
    {
        public Banco banco { private get; set; }

        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite atacar um monstro a sua frente.")]
        [ComoUsar("atacar [#ID]")]
        [ComoUsar("atacar")]
        [Exemplo("atacar #1")]
        public async Task ComandoAtacarAsync(CommandContext ctx, string stringIndexAlvo = "#0")
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.Zona.Monstros.Count == 0)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não tem vê monstros para atacar!");
                return;
            }

            // Converte o id informado.
            if (!stringIndexAlvo.TryParseID(out int indexAlvo))
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, o `#ID` precisa ser numérico!");
                return;
            }

            // Inicia uma sessão do Mongo para não ter alteração duplicada.
            using (var session = await banco.StartSessionAsync())
            {
                BancoSession banco = new BancoSession(session);
                RPJogador jogador = await banco.GetJogadorAsync(ctx);
                RPPersonagem personagem = jogador.Personagem;

                // Limita o id.
                indexAlvo = Math.Clamp(indexAlvo, 0, personagem.Zona.Monstros.Count - 1);

                StringBuilder resumoBatalha = new StringBuilder();
                bool monstroMorreu = false;

                // Executa os efeitos ativos no personagem.
                personagem.CalcEfeitos(resumoBatalha);

                // Executa os ataques do inimigo.
                personagem.Zona.CalcAtaquesInimigos(personagem, resumoBatalha);

                // Exibimos a vida/mana do personagem no começo da mensagem.
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder();


                // Verifica se o personagem vai acertar o monstro
                bool chanceAcertoPersonagem = Calculo.DanoFisicoChanceAcerto(personagem.Precisao.Modificado, personagem.Zona.Monstros[indexAlvo].Evasao);
                if (chanceAcertoPersonagem)
                {
                    // Randomizamos um dano médio com base no minimo e max da arma equipada.
                    double danoPersonagem = personagem.DanoFisicoModificado.Sortear;
                    personagem.Zona.Monstros[indexAlvo].Vida -= danoPersonagem;
                    resumoBatalha.AppendLine($"\nVocê causou {danoPersonagem.Text()} de dano no {personagem.Zona.Monstros[indexAlvo].Nome}!");

                    // Se o monstro morrer.
                    if (personagem.Zona.Monstros[indexAlvo].Vida <= 0)
                    {
                        DiscordEmoji xp = DiscordEmoji.FromGuildEmote(ctx.Client, 758439721016885308);
                        double expGanha = Calculo.CalcularEfetividadeXP(personagem.Nivel.Atual, personagem.Zona.Monstros[indexAlvo].Nivel) * personagem.Zona.Monstros[indexAlvo].Exp;
                        resumoBatalha.AppendLine($"{Emoji.CrossBone} {personagem.Zona.Monstros[indexAlvo].Nome.Bold()} ️{Emoji.CrossBone}\n" +
                            $"{xp}+{expGanha.Text()}.");
                        monstroMorreu = true;

                        int evoluiu = personagem.AddExp(expGanha);
                        if (evoluiu != 0)
                            embed.AddField("Evolução".Titulo(), $"{Emoji.Up} Você evoluiu de nível!");

                        // Recarrega os frascos
                        foreach (var item in personagem.Frascos)
                            item.AddCarga(1);
                        // Dropa itens
                        personagem.Zona.SortearItem(personagem.Zona.Monstros[indexAlvo], personagem.ChanceDrop);
                    }
                }
                else // Caso ele erre o ataque
                    resumoBatalha.AppendLine($"\nVocê errou o ataque!");

                // Informações complementares
                embed.WithDescription($"**Ondas restantes {personagem.Zona.OndaTotal - personagem.Zona.OndaAtual}.** " +
                 $"**Turno {personagem.Zona.Turno}.**\n" +
                  $"**Inimigo restante {personagem.Zona.Monstros.Count} .**");

                embed.WithColor(DiscordColor.Blue);
                // Se o personagem morrer, reseta ele.
                bool jogadorMorreu = false;
                if (personagem.Vida.Atual <= 0)
                {
                    personagem.Resetar();

                    resumoBatalha.AppendLine($"{Emoji.CrossBone} Você morreu!!! {Emoji.CrossBone}".Bold());
                    embed.WithColor(DiscordColor.Red);
                    jogadorMorreu = true;
                }

                // Exibe o resumo da batalha.. Regens e ataques.
                embed.AddField("Resumo da batalha".Titulo(), resumoBatalha.ToString());
                embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);
                embed.WithThumbnail("https://cdn.discordapp.com/attachments/758139402219159562/758425473541341214/sword-and-shield-icon-17.png");

                if (personagem.Zona.ItensNoChao.Count != 0)
                    embed.AddField("Itens no chão".Titulo().Bold(), $":mag: {personagem.Zona.ItensNoChao.Count.Bold()} item. ");

                // Adiciona nova onda.
                if (monstroMorreu)
                {
                    if (personagem.Zona.NovaOnda(personagem.VelocidadeAtaque.Modificado, out int inimigosNovos))
                        embed.AddField("Nova onda".Titulo(), $"Apareceu {inimigosNovos} monstros!");
                }
                if (!jogadorMorreu)
                {
                    embed.AddField($"{Emoji.OrbVida} {"Vida".Titulo()}", $"{personagem.Vida.Atual.Text()}/{personagem.Vida.Maximo.Text()}", true);
                    embed.AddField($"{Emoji.OrbMana} {"Mana".Titulo()}", $"{personagem.Mana.Atual.Text()}/{personagem.Mana.Maximo.Text()}", true);
                }

                // Salvamos.
                await banco.EditJogadorAsync(jogador);
                await session.CommitTransactionAsync();

                await ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());
            }
        }
    }
}
