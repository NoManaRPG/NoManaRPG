using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using WafclastRPG.DataBases;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class StatusCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("status")]
        [Aliases("s")]
        [Description("Exibe o status do seu personagem.")]
        [Usage("status")]
        public async Task StatusCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindAsync(ctx.User);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var str = new StringBuilder();

            str.AppendLine($"{player.Character.ExperienciaAtual:N2} de experiencia e precisa {(player.Character.ExperienciaProximoNivel - player.Character.ExperienciaAtual):N2} para o nível {player.Character.Level + 1}.");
            str.AppendLine($"Carregando {player.Character.Inventory.Quantity} itens.");
            str.AppendLine($"Recupera {player.Character.LifeRegen.CurrentValue:N2} vida e {player.Character.ManaRegen.CurrentValue:N2} mana em {(player.Character.RegenDate - DateTime.UtcNow).TotalSeconds:N0}s.");
            str.AppendLine($"{player.Character.Karma} pontos de Karma.");
            str.AppendLine($"{player.MonsterKill} monstros eliminado.");
            str.AppendLine($"{player.PlayerKill} jogadores eliminado.");
            str.AppendLine($"{player.Deaths} vezes morto.");

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}] ", iconUrl: ctx.User.AvatarUrl);
            embed.WithThumbnail(ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.WithDescription(str.ToString());

            embed.AddField("Ataque Físico".Titulo(), $"{Emojis.EspadasCruzadas} {player.Character.PhysicalDamage.CurrentValue:N2}", true);
            embed.AddField("Defesa Física".Titulo(), $"{Emojis.Escudo} {player.Character.Armour.CurrentValue:N2}", true);
            embed.AddField("Precisão".Titulo(), $"{Emojis.Escudo} {player.Character.Accuracy.CurrentValue:N2}", true);
            embed.AddField("Evasão".Titulo(), $"{Emojis.Escudo} {player.Character.Evasion.CurrentValue:N2}", true);

            var lifePor = player.Character.Life.CurrentValue / player.Character.Life.MaxValue;
            embed.AddField("Vida".Titulo(), $"{Emojis.GerarVidaEmoji(lifePor)} {player.Character.Life.CurrentValue:N2} / {player.Character.Life.MaxValue:N2}", true);
            embed.AddField("Mana".Titulo(), $"{player.Character.Mana.CurrentValue:N2} / {player.Character.Mana.MaxValue:N2}", true);
            embed.AddField("Escudo mágico".Titulo(), $"{player.Character.EnergyShield.CurrentValue:N2} / {player.Character.EnergyShield.MaxValue:N2}", true);

            var dg = await ctx.Client.GetGuildAsync(player.Character.Localization.ServerId, false);
            var dc = dg.GetChannel(player.Character.Localization.ChannelId);
            var invite = await dc.CreateInviteAsync(60, 0);
            embed.AddField("Localização".Titulo(), $"{Emojis.Mapa} {Formatter.MaskedUrl(dc.Name, new Uri(invite.ToString()))}");

            await ctx.ResponderAsync(embed.Build());
        }
    }
}
