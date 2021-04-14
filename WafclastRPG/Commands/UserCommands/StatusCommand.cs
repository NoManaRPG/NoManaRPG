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
using MongoDB.Driver;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class StatusCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("status")]
        [Aliases("s")]
        [Description("Exibe o status do seu personagem.")]
        [Usage("status")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task StatusCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.CollectionPlayers.Find(x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
            if (player == null)
            {
                await ctx.ResponderAsync(Messages.NaoEscreveuComecar);
                return;
            }

            var str = new StringBuilder();

            str.AppendLine($"{player.Character.CurrentExperience:N2} de experiencia e precisa {(player.Character.ExperienceForNextLevel - player.Character.CurrentExperience):N2} para o nível {player.Character.Level + 1}.");
            str.AppendLine($"{(player.Character.RegenDate - DateTime.UtcNow).TotalSeconds:N0}s para recuperar vida e mana.");
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
            embed.AddField("Mana".Titulo(), $":blue_circle: {player.Character.Mana.CurrentValue:N2} / {player.Character.Mana.MaxValue:N2}", true);
            if (player.Character.EnergyShield.MaxValue != 0)
                embed.AddField("Escudo mágico".Titulo(), $"{player.Character.EnergyShield.CurrentValue:N2} / {player.Character.EnergyShield.MaxValue:N2}", true);

            embed.AddField("Andar Atual".Titulo(), $":kaaba: {player.Character.CurrentFloor}");

            await ctx.ResponderAsync(embed.Build());
        }
    }
}
