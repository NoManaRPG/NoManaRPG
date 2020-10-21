using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using TorreRPG.Atributos;
using TorreRPG.Services;
using System;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoStatus : BaseCommandModule
    {
        public readonly Banco banco;

        [Command("status")]
        [Description("Permite exibir os status do personagem ou de outro usuário.")]
        [ComoUsar("status")]
        [ComoUsar("status [@USUARIO]")]
        [Exemplo("status @Imain")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task ComandoStatusAb(CommandContext ctx, DiscordUser discordUser)
        {
            RPJogador jogador = await banco.GetJogadorAsync(discordUser);
            if (jogador == null)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, este jogador não tem um personagem ainda! Peça-o para criar um!");
                return;
            }
            await ctx.RespondAsync(embed: (await GerarStatusAsync(discordUser)).Build());
        }

        [Command("status")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task ComandoStatusAb(CommandContext ctx)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;
            await ctx.RespondAsync(embed: (await GerarStatusAsync(ctx.User)).Build());
        }

        public async Task<DiscordEmbedBuilder> GerarStatusAsync(DiscordUser user)
        {
            RPJogador jogador = await banco.GetJogadorAsync(user);
            RPPersonagem personagem = jogador.Personagem;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{user.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: user.AvatarUrl);
            embed.WithThumbnail(jogador.UrlFoto);
            embed.WithDescription($"Tem {personagem.Nivel.ExpAtual.Text().Bold()} pontos de experiencia e precisa de {personagem.Nivel.ExpMax.Text().Bold()} para evoluir.\n" +
                $"Mochila com **{personagem.Mochila.Espaco}/64** de espaço.\n" +
                $"Regenera {personagem.Vida.RegenPorSegundo.Text().Bold()} pontos vida por segundo.\n" +
                $"Regenera {personagem.Mana.RegenPorSegundo.Text().Bold()} pontos mana por segundo.\n" +
                $"Tem {personagem.Evasao.Modificado.Text().Bold()} pontos de evasão.\n" +
                $"Tem {personagem.Precisao.Modificado.Text().Bold()} pontos de precisão.\n" +
                $"Tem {personagem.Armadura.Modificado.Text().Bold()} pontos de armadura.\n");

            embed.AddField($"{Emoji.OrbVida} {"Vida".Titulo()}", $"{personagem.Vida.Atual.Text()}/{personagem.Vida.Maximo.Text()}", true);
            embed.AddField($"{Emoji.OrbMana} {"Mana".Titulo()}", $"{personagem.Mana.Atual.Text()}/{personagem.Mana.Maximo.Text()}", true);
            embed.AddField("Dano por segundo".Titulo(), $"{((personagem.DanoFisicoModificado.Maximo + personagem.DanoFisicoModificado.Minimo / 2) * personagem.VelocidadeAtaque.Modificado).Text()}");
            embed.AddField("Dano físico combinado".Titulo(), $"{personagem.DanoFisicoModificado.Minimo} - {personagem.DanoFisicoModificado.Maximo}", true);

            return embed;
        }
    }
}
