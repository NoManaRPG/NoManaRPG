using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using TorreRPG.Services;
using System;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoMonstros : BaseCommandModule
    {
        public Banco banco;

        [Command("monstros")]
        [Description("Permite ver os monstros que estão na sua frente.")]
        public async Task ComandoMonstrosAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            if (personagemNaoModificar.IsPortalAberto)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não pode usar este comando com o portal aberto!");
                return;
            }

            if (personagemNaoModificar.Zona.Monstros == null)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você não está em combate! Explore uma zona!");
                return;
            }

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - {personagemNaoModificar.Nome}", iconUrl: ctx.User.AvatarUrl);

            for (int i = 0; i < personagemNaoModificar.Zona.Monstros.Count; i++)
            {
                var monstro = personagemNaoModificar.Zona.Monstros[i];
                embed.AddField($"`#{i}`{monstro.Nome.Titulo().Bold()}", $"{monstro.Vida.Text()} vida.", true);
            }
            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
