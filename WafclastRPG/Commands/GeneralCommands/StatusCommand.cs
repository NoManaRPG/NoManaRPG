using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class StatusCommand : BaseCommandModule
    {
        public Database banco;

        [Command("status")]
        [Aliases("s")]
        [Description("Exibe o status do seu personagem.")]
        [Usage("status")]
        public async Task StatusCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx.User.Id);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var str = new StringBuilder();

            str.AppendLine($"Tem {player.Character.ExperienciaAtual:N2} pontos de experiencia e precisa de {(player.Character.ExperienciaProximoNivel - player.Character.ExperienciaAtual):N2} para o nível {player.Character.Level + 1}.");
            str.AppendLine($"Está carregando 0 itens.");
            str.AppendLine($"Regenerá {(player.Character.Atributos.Vitalidade * 0.2m):N2} pontos de vida na prox mensagem ({(player.Character.RegenDate - DateTime.UtcNow).TotalSeconds:N0}s).");
            str.AppendLine($"Está com {player.Character.Karma} pontos de Karma.");

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}] ", iconUrl: ctx.User.AvatarUrl);
            embed.WithThumbnail(ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.WithDescription(str.ToString());

            embed.AddField("Ataque".Titulo(), $"{Emojis.EspadasCruzadas} {player.Character.Ataque:N2}", true);
            embed.AddField("Defesa".Titulo(), $"{Emojis.Escudo} {player.Character.Defesa:N2}", true);
            embed.AddField(":white_large_square:", ":white_large_square:");

            var lifePor = player.Character.LifePoints.CurrentValue / player.Character.LifePoints.MaxValue;
            embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(lifePor)} {player.Character.LifePoints.CurrentValue:N2} / {player.Character.LifePoints.MaxValue:N2}",true);
            embed.AddField("Estamina".Titulo(), $"{player.Character.Stamina.CurrentValue:N2} / {player.Character.Stamina.MaxValue:N2}",true);

            var dg = await ctx.Client.GetGuildAsync(player.Character.Localization.ServerId, false);
            var dc = dg.GetChannel(player.Character.Localization.ChannelId);
            var invite = await dc.CreateInviteAsync(60, 0);
            embed.AddField("Localização".Titulo(), $"{Emojis.Mapa} {Formatter.MaskedUrl(dc.Name, new Uri(invite.ToString()))}");

            await ctx.ResponderAsync(embed.Build());
        }
    }
}
