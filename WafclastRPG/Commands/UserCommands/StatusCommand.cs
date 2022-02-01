// This file is part of WafclastRPG project.

using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using WafclastRPG.Attributes;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class StatusCommand : BaseCommandModule
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IZoneRepository _zoneRepository;

        public StatusCommand(IPlayerRepository playerRepository, IMongoSession session, IZoneRepository zoneRepository)
        {
            this._playerRepository = playerRepository;
            this._zoneRepository = zoneRepository;
        }

        [Command("status")]
        [Description("Permite visualizar dados do seu personagem.")]
        [Usage("status")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task StatusCommandAsync(CommandContext ctx)
        {
            var player = await this._playerRepository.FindPlayerAsync(ctx);
            var character = player.Character;
            var str = new StringBuilder();

            str.AppendLine($"{player.CurrentExperience:N2} de experiencia e precisa {(player.ExperienceForNextLevel - player.CurrentExperience):N2} para o nível {player.Level + 1}.");
            str.AppendLine($"{player.MonstersKills} monstros abatidos.");
            str.AppendLine($"{player.Deaths} vezes morto.");
            //str.AppendLine($"{player.Energy.Current}/{player.Energy.Max} energia disponível.");

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Level}] ", iconUrl: ctx.User.AvatarUrl);
            embed.WithThumbnail(ctx.User.AvatarUrl);
            embed.WithColor(DiscordColor.Blue);
            embed.WithDescription(str.ToString());

            //embed.AddField($"Dano [Nv.{character.AttackPointsLevel} R{character.AttackPointsRank}]", $"{Emojis.EspadasCruzadas} {character.AttackPoints:N2}", true);
            //embed.AddField($"Vida [Nv.{character.LifePointsLevel} R{character.LifePointsRank}]", $"{Emojis.CoracaoVermelho} {character.LifePoints.Max:N2}", true);

            var zone = await this._zoneRepository.FindPlayerHighestZoneAsync(player.DiscordId);
            if (zone != null)
                embed.AddField("Maior Zona", $"{Emojis.Mapa} {zone.Level}");


            await ctx.RespondAsync(embed);
        }
    }
}
