using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Enuns;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos.Exibir
{
    public class ComandoMochila : BaseCommandModule
    {
        [Command("mochila")]
        public async Task ComandoMochilaAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < personagem.Mochila.Itens.Count; i++)
                str.AppendLine($"*#{i}* - {personagem.Mochila.Itens[i].Nome.Titulo().Bold()} ");


            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe}", iconUrl: ctx.User.AvatarUrl);
            embed.WithDescription($"**Espaço da mochila: {personagem.Mochila.Espaco}/64**\n\n{str}");

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
