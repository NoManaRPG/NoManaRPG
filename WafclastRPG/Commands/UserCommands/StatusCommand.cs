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

            str.AppendLine($"{player.MonsterKills} monstros eliminado.");
            str.AppendLine($"{player.PlayerKills} jogadores eliminado.");
            str.AppendLine($"{player.Deaths} vezes abatido.");

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username}", iconUrl: ctx.User.AvatarUrl);
            embed.WithThumbnail(ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.WithDescription(str.ToString());

            //embed.AddField("Ataque".Title(), $"{Emojis.EspadasCruzadas} {player.Character.MaxDamage:N2}", true);
            embed.AddField("Defesa".Title(), $"{Emojis.Escudo} {player.Character.Armor:N2}", true);
            //embed.AddField("Precisão".Title(), $"{Emojis.Escudo} {player.Character.Accuracy.CurrentValue:N2}", true);
            //embed.AddField("Evasão".Title(), $"{Emojis.Escudo} {player.Character.Evasion.CurrentValue:N2}", true);

            var lifePor = player.Character.LifePoints.BaseValue / player.Character.LifePoints.CurrentValue;
            embed.AddField("Vida".Title(), $"{Emojis.GerarVidaEmoji(lifePor)} {player.Character.LifePoints.BaseValue:N2} / {player.Character.LifePoints.CurrentValue:N2}", true);

            //            embed.AddField("Andar Atual".Title(), $":kaaba: {player.Character.CurrentFloor}");

            await ctx.ResponderAsync(embed.Build());
        }
    }
}
