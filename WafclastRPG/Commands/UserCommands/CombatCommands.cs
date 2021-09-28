using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class CombatCommands : BaseCommandModule {
    private Response _res;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMongoSession _session;

    public CombatCommands(IPlayerRepository playerRepository, IRoomRepository roomRepository, IMongoSession session) {
      _playerRepository = playerRepository;
      _roomRepository = roomRepository;
      _session = session;
    }

    [Command("explorarSession")]
    [Description("Permite explorar uma região, podendo encontrar monstros.")]
    [Usage("explorar")]
    public async Task BasicAtasdtackCommandAsync(CommandContext ctx) {
      using (var session = await _session.StartSession())
        _res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          //Combat
          var combatResult = player.BasicAttackMonster();
          if (combatResult == "você não está visualizando nenhum monstro para atacar!")
            return new Response(combatResult);

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
    public async Task BasicAttackCommandAsync(CommandContext ctx) {
      using (var sessionHandler = await _session.StartSession())
        _res = await sessionHandler.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          //Combat
          var combatResult = player.BasicAttackMonster();
          if (combatResult == "você não está visualizando nenhum monstro para atacar!")
            return new Response(combatResult);

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

    public async Task ExploreCommandAsync(CommandContext ctx) {
      using (var sessionHandler = await _session.StartSession())
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
