using TorreRPG.Entidades;
using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;
using TorreRPG.Services;
using System;

namespace TorreRPG.Comandos.Exibir
{
    public class ComandoEquipamentos : BaseCommandModule
    {
        public Banco banco;

        [Command("equipamentos")]
        [Aliases("eq")]
        [Description("Permite todos os itens equipados no seu personagem. Cada item está separado por `⌈SLOT⌋`.")]
        public async Task ComandoEquipamentosAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador,
            var (naoCriouPersonagem, personagemNaoModificar) = await banco.VerificarJogador(ctx);
            if (naoCriouPersonagem) return;

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} - Nível {personagemNaoModificar.Nivel.Atual} - {personagemNaoModificar.Classe} - {personagemNaoModificar.Nome}", iconUrl: ctx.User.AvatarUrl);

            embed.AddField("Primeira mão".Titulo().Bold(), $"{(personagemNaoModificar.MaoPrincipal == null ? "Nada equipado" : personagemNaoModificar.MaoPrincipal.TipoBaseModificado)}", true);
            embed.AddField("Segunda mão".Titulo().Bold(), $"{(personagemNaoModificar.MaoSecundaria == null ? "Nada equipado" : personagemNaoModificar.MaoSecundaria.TipoBaseModificado)}", true);


            StringBuilder str = new StringBuilder();
            for (int i = 0; i < personagemNaoModificar.Frascos.Count; i++)
            {
                var pocao = personagemNaoModificar.Frascos[i];
                str.AppendLine($"`#{i}` {pocao.TipoBaseModificado}: {pocao.CargasAtual}/{pocao.CargasMax}");
            }
            embed.AddField("Poções".Titulo().Bold(), $"{(personagemNaoModificar.Frascos.Count == 0 ? "Nada equipado" : str.ToString())}");

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
