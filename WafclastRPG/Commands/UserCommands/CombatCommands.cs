using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;
using WafclastRPG.Repositories.Interfaces;
using static WafclastRPG.Mathematics;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class CombatCommands : BaseCommandModule {
    private Response _res;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;

    public CombatCommands(IPlayerRepository playerRepository, IRoomRepository roomRepository) {
      _playerRepository = playerRepository;
      _roomRepository = roomRepository;
    }

    [Command("ataque-basico")]
    [Aliases("at", "attack", "ab")]
    [Description("Permite executar um ataque em um monstro.")]
    [Usage("atacar")]
    public async Task BasicAttackCommandAsync(CommandContext ctx) {
      using (var sessionHandler = (SessionHandler) await _playerRepository.StartSession())
        _res = await sessionHandler.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          //Combat
          var combatResult = player.BasicAttackMonster();
          await _playerRepository.SavePlayerAsync(player);

          var embed = new DiscordEmbedBuilder();
          embed.AddField(ctx.User.Username, $"{Emojis.TesteEmoji(player.Character.LifePoints)} {player.LifePoints}", true);

          embed.WithColor(DiscordColor.Red);
          embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
          embed.WithTitle("Relatório de Combate");
          embed.WithDescription(combatResult.ToString());

          var monster = player.Character.Room.Monster;

          embed.AddField(monster.Mention, $"{Emojis.GerarVidaEmoji(monster.LifePoints)} {monster.LifePoints.Current:N2} ", true);

          //loot em outro comando!

          return new Response(embed);
        });
      await ctx.RespondAsync(_res);
    }

    [Command("explorar")]
    [Aliases("ex", "explore")]
    [Description("Permite explorar uma região, podendo encontrar monstros.")]
    [Usage("explorar")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public async Task ExploreCommandAsync(CommandContext ctx) {
      using (var sessionHandler = (SessionHandler) await _playerRepository.StartSession())
        _res = await sessionHandler.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          var character = player.Character;
          character.Room = await _roomRepository.FindRoomOrDefaultAsync(character.Room.Id);

          if (character.Room.Monster == null) {
            return new Response("você procura monstros para atacar.. mas parece que não você não encontrará nada aqui.");
          }

          await _playerRepository.SavePlayerAsync(player);

          return new Response($"você encontrou **[{character.Room.Monster.Mention}]!**");
        });
      await ctx.RespondAsync(_res);
    }
  }
}
