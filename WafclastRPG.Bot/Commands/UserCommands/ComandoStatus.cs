using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Extensoes;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;
using WafclastRPG.Game.Extensoes;

namespace WafclastRPG.Bot.Comandos.Exibir
{
    public class ComandoStatus : BaseCommandModule
    {
        public Banco banco;

        [Command("status")]
        [Description("Permite exibir os status do personagem ou de outro usuário.")]
        [Example("status <@87604980344721408>", "Exibe os status do <@87604980344721408>.")]
        [Example("status", "Exibe os seus status.")]
        [Usage("status [ @jogador ]")]
        public async Task ComandoStatusAb(CommandContext ctx, DiscordUser user)
        {
            var jogador = await banco.GetJogadorAsync(user);
            if (jogador == null)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, este jogador não tem um personagem ainda! Peça-o para criar um!");
                return;
            }
            var embed = GerarStatus(user, jogador.Personagem);
            await ctx.RespondAsync(embed: embed.Build());
        }

        [Command("status")]
        [Cooldown(1, 10, CooldownBucketType.User)]
        public async Task ComandoStatusAsync(CommandContext ctx)
        {
            // Verifica se existe o jogador,
            var (isJogadorCriado, sessao) = await banco.ExisteJogadorAsync(ctx);
            if (!isJogadorCriado) return;

            var embed = GerarStatus(ctx.User, sessao.Jogador.Personagem);
            await ctx.RespondAsync(embed: embed.Build());
        }

        public DiscordEmbedBuilder GerarStatus(DiscordUser user, WafclastPersonagem personagem)
        {
            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{user.Username} [Lvl.{personagem.Nivel.Atual}] ({personagem.Classe.GetEnumDescription()})", "https://discord.gg/MAR4NFq", user.AvatarUrl);
            embed.WithFooter("Se estiver perdido use o comando ajuda.", "https://cdn.discordapp.com/attachments/736163626934861845/742671714386968576/help_animated_x4_1.gif");
            var str = new StringBuilder();
            str.AppendLine($"Tem {personagem.Nivel.ExpAtual.ToString("N2").Bold()} pontos de experiencia e precisa de {personagem.Nivel.ExpMax.ToString("N2").Bold()} para evoluir.");
            str.AppendLine($"Mochila com {personagem.Mochila.EspacoAtual.Bold()}/{personagem.Mochila.EspacoMax.Bold()} de espaço.");
            str.AppendLine($"Regenera {personagem.Vida.RegenPorSegundo.ToString("N2").Bold()} pontos vida a cada 30 segundos.");
            str.AppendLine($"Regenera {personagem.Mana.RegenPorSegundo.ToString("N2").Bold()} pontos mana a cada 30 segundos.");
            str.AppendLine($"Tem {personagem.Evasao.Calculado.ToString("N2").Bold()} pontos de evasão.");
            str.AppendLine($"Tem {personagem.Precisao.Calculado.ToString("N2").Bold()} pontos de precisão.");
            str.AppendLine($"Tem {personagem.Armadura.Calculado.ToString("N2").Bold()} pontos de armadura.");
            embed.WithDescription(str.ToString());
            embed.WithThumbnail(user.AvatarUrl);
            embed.AddField($"{WafclastPersonagem.VidaEmoji(personagem.Vida.Atual / personagem.Vida.Maximo)} {"Vida".Titulo()}", $"{personagem.Vida.Atual:N2}/{personagem.Vida.Maximo:N2}", true);
            embed.AddField($"{WafclastPersonagem.ManaEmoji(personagem.Mana.Atual / personagem.Mana.Maximo)} {"Mana".Titulo()}", $"{personagem.Mana.Atual:N2}/{personagem.Mana.Maximo:N2}", true);
            embed.AddField($"{Emoji.Fome} {"Fome".Titulo()}", $"{personagem.Fome.Atual:N2}/{personagem.Fome.Maximo:N2}", true);
            embed.AddField($"{Emoji.Sede} {"Sede".Titulo()}", $" {personagem.Sede.Atual:N2}/{personagem.Sede.Maximo:N2}", true);
            embed.AddField($":zap: {"Vigor".Titulo()}", $" {personagem.GetVigor():N2}/{personagem.Vigor.Maximo:N2}", true);
            var danoFisico = personagem.DanoFisicoCalculado;
            embed.AddField($"{Emoji.EspadasCruzadas} {"Dano físico".Titulo()}", $"{danoFisico.Minimo:N2} - {danoFisico.Maximo:N2}", true);

            return embed;
        }
    }
}
