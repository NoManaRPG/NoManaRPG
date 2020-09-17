using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoEquipamentos : BaseCommandModule
    {
        [Command("equipamentos")]
        [Aliases("eq")]
        public async Task ComandoEquipamentosAsync(CommandContext ctx)
        {
            var jogadorNaoExisteAsync = await ctx.JogadorNaoExisteAsync();
            if (jogadorNaoExisteAsync) return;

            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            RPPersonagem personagem = jogador.Personagem;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagem.Nivel.Atual} - {personagem.Classe} - {personagem.Nome}", iconUrl: ctx.User.AvatarUrl);

            embed.AddField("Primeira mão".Titulo().Bold(), $"{(personagem.MaoPrincipal == null ? "Nada equipado" : personagem.MaoPrincipal.TipoBaseModificado)}", true);
            embed.AddField("Segunda mão".Titulo().Bold(), $"{(personagem.MaoSecundaria == null ? "Nada equipado" : personagem.MaoSecundaria.TipoBaseModificado)}", true);


            StringBuilder str = new StringBuilder();
            for (int i = 0; i < personagem.Frascos.Count; i++)
            {
                var pocao = personagem.Frascos[i];
                str.AppendLine($"*#{i}* - {pocao.TipoBaseModificado}: {pocao.CargasAtual}/{pocao.CargasMax}");
            }
            embed.AddField("Poções".Titulo().Bold(), $"{(personagem.Frascos.Count == 0 ? "Nada equipado" : str.ToString())}");

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
