// This file is part of WafclastRPG project.

using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using NoManaRPG.Attributes;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Extensions;

namespace NoManaRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class StatusCommand : BaseCommandModule
    {
        private readonly PlayerRepository _playerRepository;
        private readonly ZoneRepository _zoneRepository;

        public StatusCommand(PlayerRepository playerRepository, MongoSession session, ZoneRepository zoneRepository)
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

            str.AppendLine($"{player.CurrentExperience:N2} de experiencia e precisa {(player.ExperienceForNextLevel - player.CurrentExperience):N2} para o nível {player.ActualLevel + 1}.");
            str.AppendLine($"{player.MonstersKills} monstros abatidos.");
            str.AppendLine($"{player.Deaths} vezes morto.");
            //str.AppendLine($"{player.Energy.Current}/{player.Energy.Max} energia disponível.");

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.ActualLevel}] ", iconUrl: ctx.User.AvatarUrl);
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
