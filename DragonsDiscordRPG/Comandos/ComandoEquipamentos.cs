using DragonsDiscordRPG.Entidades;
using DragonsDiscordRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoEquipamentos : BaseCommandModule
    {
        [Command("equipamentos")]
        public async Task ComandoEquipamentosAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);

            embed.AddField("Primeira mão".Titulo().Bold(), $"{(personagem.MaoPrincipal == null ? "Nada equipado" : personagem.MaoPrincipal.Nome)}", true);
            embed.AddField("Segunda mão".Titulo().Bold(), $"{(personagem.MaoSecundaria == null ? "Nada equipado" : personagem.MaoSecundaria.Nome)}", true);


            StringBuilder str = new StringBuilder();
            for (int i = 0; i < personagem.Pocoes.Count; i++)
            {
                var pocao = personagem.Pocoes[i];
                str.AppendLine($"*#{i}* - {pocao.Nome}: {pocao.CargasAtual}/{pocao.CargasMax}");
            }
            embed.AddField("Poções".Titulo().Bold(), $"{(personagem.Pocoes.Count == 0 ? "Nada equipado" : str.ToString())}");

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
