using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class ComandoHabilidade : BaseCommandModule
    {
        public Banco banco;
        public Config config;

        [Command("habilidade")]
        [Description("Exibe o nivel nome e experiencia das suas habilidades")]
        [Usage("habilidade [ habilidade ]")]
        [Example("habilidade", "Mostra todas as suas habilidades e o respectivo nível.")]
        [Example("habilidade ataque", "Exibe todas as informações da habilidade ataque. Como experiencia atual e próximo nível.")]
        public async Task ComandoHabilidadesAsync(CommandContext ctx, string hab = "")
        {
            var jogador = await banco.GetJogadorAsync(ctx);
            var per = jogador.Personagem;

            var embed = new DiscordEmbedBuilder().Inicializar(ctx);

            if (string.IsNullOrEmpty(hab))
            {
                var str = new StringBuilder();
                foreach (var item in per.Habilidades)
                    str.AppendLine($"**{item.Key.GetEnumDescription()}** - Nível {item.Value.Nivel}.");
                embed.WithDescription($"Digite `{config.PrefixRelease}habilidade ataque` para saber mais sobre a habilidade em especifico. \n\n{str.ToString()}");
            }
            else
            {
                hab = hab.RemoverAcentos();
                if (Enum.TryParse<ProficienciaType>(hab, true, out var habilidade))
                {
                    embed.WithTitle(habilidade.GetEnumDescription().Titulo().Bold());
                    var habPer = per.GetHabilidade(habilidade);
                    embed.WithDescription($"{habPer.Descricao}");
                    embed.AddField("Nível", habPer.Nivel.ToString());
                    embed.AddField("EXP atual", habPer.ExperienciaAtual.ToString());
                    embed.AddField("Próximo nível", habPer.ExperienciaProximoNivel.ToString());
                    embed.AddField("Restantes", (habPer.ExperienciaProximoNivel - habPer.ExperienciaAtual).ToString());
                }
                else
                {
                    await ctx.RespondAsync($"{ctx.User.Mention}, você informou uma habilidade que não existe!");
                    return;
                }
            }

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
