using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Entities.Wafclast;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WafclastRPG.Exceptions;
using WafclastRPG.Extensions;
using DSharpPlus.Interactivity.Extensions;
using WafclastRPG.Repositories.Interfaces;
using WafclastRPG.Context;
using WafclastRPG.Entities;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class LookAroundCommand : BaseCommandModule {
    private Response _res;
    private readonly IPlayerRepository _playerRepository;
    private readonly MongoDbContext _mongoDbContext;
    private readonly IRoomRepository _roomRepository;
    private readonly Config _config;

    public LookAroundCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, Config config,MongoDbContext mongoDbContext) {
      _playerRepository = playerRepository;
      _mongoDbContext = mongoDbContext;
      _roomRepository = roomRepository;
      _config = config;
    }

    [Command("olhar")]
    [Aliases("look", "lookaround")]
    [Description("Permite olhar ao seu redor e outro local.")]
    [Usage("olhar [ nome ]")]
    public async Task LookAroundCommandAsync(CommandContext ctx, [RemainingText] string roomName) {

      var player = await _mongoDbContext.Players.Find(x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
      if (player == null)
        throw new PlayerNotCreatedException();

      Room room = null;

      if (string.IsNullOrWhiteSpace(roomName)) {
        room = await _mongoDbContext.Rooms.Find(x => x.Id == player.Character.Room.Id).FirstOrDefaultAsync();
        if (room == null) {
          await ctx.ResponderAsync("parece que aconteceu algum erro na matrix!");
          return;
        }
      } else {
        room = await _mongoDbContext.Rooms.Find(x => x.Name == roomName, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
        if (room == null) {
          await ctx.ResponderAsync("você tenta procurar no mapa o lugar, mas não encontra! Como você tentaria olhar para algo que você não conhece?!");
          return;
        }
      }

      var embed = new DiscordEmbedBuilder();
      embed.WithColor(DiscordColor.Blue);
      embed.WithTitle(room.Name);
      embed.WithDescription(room.Description);
      var msg = await ctx.RespondAsync(embed.Build());

      if (room != player.Character.Room)
        if (room.Location.Distance(player.Character.Room.Location) <= 161) {
          var emoji = DiscordEmoji.FromName(ctx.Client, ":footprints:");
          await msg.CreateReactionAsync(emoji);
          var vity = ctx.Client.GetInteractivity();
          var click = await vity.WaitForReactionAsync(x => x.User.Id == ctx.User.Id && x.Message.Id == msg.Id && x.Emoji == emoji);
          if (click.TimedOut)
            return;

          await TravelCommandAsync(ctx, room.Name);
        }
    }


    [Command("viajar")]
    [Aliases("v", "travel")]
    [Description("Permite viajar para outro local.")]
    [Usage("viajar [ nome ]")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public async Task TravelCommandAsync(CommandContext ctx, [RemainingText] string roomName) {
      using (var sessionHandler = (SessionHandler) await _playerRepository.StartSession())
        _res = await sessionHandler.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          var character = player.Character;

          Room room = null;

          if (string.IsNullOrWhiteSpace(roomName)) {
            room = await _roomRepository.FindRoomOrDefaultAsync(ctx.Channel.Id);
            if (room == null)
              return new Response("você foi para algum lugar, talvez alguns passos a frente.");
          } else {
            room = await _roomRepository.FindRoomOrDefaultAsync(roomName);
            if (room == null)
              return new Response("você tenta procurar no mapa o lugar, mas não encontra! Como você chegaria em um lugar em que você não conhece?!");
          }

          if (room.Location.Distance(character.Room.Location) > 161)
            return new Response("parece ser um caminho muito longe! Melhor tentar algo mais próximo.");
          if (room == player.Character.Room)
            return new Response("como é bom estar no lugar que você sempre quis...");

          room.Monster = null;
          character.Room = room;
          await _playerRepository.SavePlayerAsync(player);

          return new Response($"você chegou em: **[{room.Name}]!**");
        });
      await ctx.ResponderAsync(_res);
    }

    [Command("mapa")]
    [Aliases("map")]
    [Description("Permite ver o mapa para todas os locais disponíveis.")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    [Usage("mapa")]
    public async Task MapCommandAsync(CommandContext ctx) {
      await ctx.TriggerTypingAsync();
      await ctx.RespondAsync(_config.MapUrl);
    }
  }
}
