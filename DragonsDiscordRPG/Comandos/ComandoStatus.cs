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
    public class ComandoStatus : BaseCommandModule
    {
        [Command("status")]
        [Description("Permite exibir os status do personagem.")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task ComandoStatusAb(CommandContext ctx, DiscordUser discordUser)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;
            await ctx.RespondAsync(embed: (await GerarStatusAsync(discordUser)).Build());
        }

        [Command("status")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task ComandoStatusAb(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;
            await ctx.RespondAsync(embed: (await GerarStatusAsync(ctx.User)).Build());
        }

        public async Task<DiscordEmbedBuilder> GerarStatusAsync(DiscordUser user)
        {
            RPJogador jogador = await ModuloBanco.GetJogadorAsync(user);
            RPPersonagem personagem = jogador.Personagem;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{user.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: user.AvatarUrl);

            embed.WithDescription($"Tem {personagem.Nivel.ExpAtual.Text().Bold()} pontos de experiencia e precisa de {personagem.Nivel.ExpMax.Text().Bold()} para evoluir.\n" +
                $"Mochila com **{personagem.Mochila.Espaco}/64** de espaço.\n" +
                $"Regenera {personagem.Vida.RegenPorSegundo.Text().Bold()} pontos vida por segundo.\n" +
                $"Regenera {personagem.Mana.RegenPorSegundo.Text().Bold()} pontos mana por segundo.\n" +
                $"Tem {personagem.Evasao.Atual.Text().Bold()} pontos de evasão.\n" +
                $"Tem {personagem.Precisao.Atual.Text().Bold()} pontos de precisão.\n" +
                $"Tem {personagem.Armadura.Atual.Text().Bold()} pontos de armadura.\n");

            embed.AddField($"{Emoji.OrbVida} {"Vida".Titulo()}", $"{personagem.Vida.Atual.Text()}/{personagem.Vida.Maximo.Text()}", true);
            embed.AddField($"{Emoji.OrbMana} {"Mana".Titulo()}", $"{personagem.Mana.Atual.Text()}/{personagem.Mana.Maximo.Text()}", true);
            embed.AddField("Dano por segundo".Titulo(), $"{((personagem.DanoFisico.Maximo + personagem.DanoFisico.Minimo / 2) * personagem.VelocidadeAtaque.Atual).Text()}");
            embed.AddField("Dano físico combinado".Titulo(), $"{personagem.DanoFisico.Minimo} - {personagem.DanoFisico.Maximo}", true);

            return embed;
        }
    }
}
