using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades.Itens;
using WafclastRPG.Game.Entidades.Proficiencias;
using WafclastRPG.Game.Enums;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoStatus : BaseCommandModule
    {
        public Banco banco;

        [Command("status")]
        [Description("Exibe os status do seu personagem.")]
        [Example("status", "Exibe os seus status.")]
        public async Task ComandoStatusAsync(CommandContext ctx)
        {
            var jog = await banco.GetJogadorAsync(ctx);
            var per = jog.Personagem;
            var embed = jog.CriarEmbed();

            embed.WithThumbnail(ctx.User.AvatarUrl);

            if (per.TryGetEquipamento(EquipamentoType.PrimeiraMao, out var item))
            {
                var maoPrimaria = item as WafclastItemArma;
                var str = new StringBuilder();

                str.AppendLine($"Dano: {maoPrimaria.DanoMax}");
                str.AppendLine($"Precisão: {maoPrimaria.Precisao}");
                str.AppendLine($"Velocidade: {maoPrimaria.AtaqueVelocidadeMax}");
                embed.AddField("Mão Primária", str.ToString(), true);
            }

            if (per.TryGetEquipamento(EquipamentoType.SegundaMao, out item))
            {
                if (item is WafclastItemArma)
                {
                    var maoSec = item as WafclastItemArma;
                    var str = new StringBuilder();

                    str.AppendLine($"Dano: {maoSec.DanoMax}");
                    str.AppendLine($"Precisão: {maoSec.Precisao}");
                    str.AppendLine($"Velocidade: {maoSec.AtaqueVelocidadeMax}");
                    embed.AddField("Mão Sec.", str.ToString(), true);
                }
            }

            var perVida = per.GetHabilidade(ProficienciaType.Constituicao) as WafclastProficienciaConstituicao;
            embed.AddField($"{Emoji.GerarVidaEmoji((double)perVida.Vida / perVida.CalcularVida())} {"Vida".Titulo()}", $"{ perVida.Vida}/{ perVida.CalcularVida()}");

            embed.AddField($"_Moedas_", $"{Emoji.Coins} {per.PortaNiqueis}");

            await ctx.RespondAsync(embed: embed.Build());
        }
    }
}
