using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Attributes;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game;

namespace WafclastRPG.Bot.Commands.GeneralCommands
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

            str.AppendLine($"Tem {player.Character.ExperienciaAtual} pontos de experiencia e precisa de {player.Character.ExperienciaProximoNivel - player.Character.ExperienciaAtual} para o nível {player.Character.Level + 1}.");
            str.AppendLine($"Está carregando 0 itens.");
            str.AppendLine($"Regenerá {player.Character.Atributo.Vitalidade * 0.2m} pontos de vida na prox mensagem ({(player.Character.RegenDate - DateTime.UtcNow).TotalSeconds:N0}s).");
            str.AppendLine($"Está com {player.Character.Karma} pontos de Karma.");

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}] ", iconUrl: ctx.User.AvatarUrl);
            embed.WithThumbnail(ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.WithDescription(str.ToString());

            embed.AddField("Ataque".Titulo(), $"{Emojis.EspadasCruzadas} {player.Character.Ataque:N2}", true);
            embed.AddField("Defesa".Titulo(), $"{Emojis.Escudo} {player.Character.Defesa:N2}", true);

            var lifePor = player.Character.VidaAtual / player.Character.VidaMaxima;
            embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(lifePor)} {player.Character.VidaAtual:N2} / {player.Character.VidaMaxima:N2}");

            var dg = await ctx.Client.GetGuildAsync(player.Character.ServerId, false);
            var dc = dg.GetChannel(player.Character.LocalId);
            var invite = await dc.CreateInviteAsync(60, 0);
            embed.AddField("Localização".Titulo(), $"{Emojis.Mapa} {Formatter.MaskedUrl(dc.Name, new Uri(invite.ToString()))}");

            await ctx.ResponderAsync(embed.Build());
        }
    }
}
