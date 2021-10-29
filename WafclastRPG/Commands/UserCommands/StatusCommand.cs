// This file is part of the WafclastRPG project.

using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using WafclastRPG.Attributes;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class StatusCommand : BaseCommandModule
    {
        private IResponse _res;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMongoSession _session;

        public StatusCommand(IPlayerRepository playerRepository, IMongoSession session)
        {
            this._playerRepository = playerRepository;
            this._session = session;
        }

        [Command("status")]
        [Description("Permite visualizar dados do seu personagem.")]
        [Usage("status")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task StatusCommandAsync(CommandContext ctx)
        {
            using (await this._session.StartSessionAsync())
                this._res = await this._session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await this._playerRepository.FindPlayerAsync(ctx);

                    var character = player.Character;

                    var str = new StringBuilder();

                    //str.AppendLine($"{character.CurrentExperience:N2} de experiencia e precisa {(player.Character.ExperienceForNextLevel - player.Character.CurrentExperience):N2} para o nível {player.Character.Level + 1}.");
                    //str.AppendLine($"{player.Character.MonsterKills} monstros eliminado.");
                    //str.AppendLine($"{player.Character.Deaths} vezes abatido por monstros.");

                    var embed = new DiscordEmbedBuilder();
                    //embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}] ", iconUrl: ctx.User.AvatarUrl);
                    embed.WithThumbnail(ctx.User.AvatarUrl);
                    embed.WithColor(DiscordColor.Blue);
                    embed.WithDescription(str.ToString());

                    //embed.AddField("Dano", $"{Emojis.EspadasCruzadas} {character.Damage:N2}", true);
                    //embed.AddField("Armadura", $"{Emojis.Escudo} 0", true);
                    //embed.AddField("Precisão", $"{Emojis.Escudo} {character.PrecisionPoints:N2}", true);
                    //embed.AddField("Evasão", $"{Emojis.Escudo} {character.EvasionPoints:N2}", true);

                    var lifePor = character.LifePoints;
                    embed.AddField("Vida", $"{Emojis.GerarVidaEmoji(lifePor)} {character.LifePoints.Current:N2} / {character.LifePoints.Max:N2}", true);

                    //embed.AddField("Mana".Titulo(), $":blue_circle: {player.Character.Mana.CurrentValue:N2} / {player.Character.Mana.MaxValue:N2}", true);
                    //if (player.Character.EnergyShield.MaxValue != 0)
                    //  embed.AddField("Escudo mágico".Titulo(), $"{player.Character.EnergyShield.CurrentValue:N2} / {player.Character.EnergyShield.MaxValue:N2}", true);


                    //embed.AddField("Localização", $"{Emojis.Mapa} {Formatter.MaskedUrl(character.Room.Name, new Uri(character.Room.Invite))}");

                    return new EmbedResponse(embed);
                });
            await ctx.RespondAsync(this._res);
        }
    }
}
