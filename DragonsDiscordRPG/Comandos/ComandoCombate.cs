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
    public class ComandoCombate : BaseCommandModule
    {
        [Command("combate")]
        public async Task ComandoCombateAsync(CommandContext ctx)
        {
            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            if (personagem.Zona.Monstros == null)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não está em combate!");
                return;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);
            embed.WithDescription($"Você está batalhando contra {personagem.Zona.Monstros.Count.Bold()} monstros.\n" +
                $"Onda {personagem.Zona.OndaAtual.Bold()}/{personagem.Zona.OndaTotal.Bold()}.");

            for (int i = 0; i < personagem.Zona.Monstros.Count; i++)
            {
                var item = personagem.Zona.Monstros[i];
                embed.AddField($"{item.Nome.Titulo().Bold()} ID #{i}", $"{item.Vida.Text()} vida.", true);
            }
            await ctx.RespondAsync(embed: embed.Build());

        }
    }
}
