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
using WafclastRPG.Repositories.Interfaces;
using WafclastRPG.Context;
using System.Text;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class LookAroundCommand : BaseCommandModule {
    private Response _res;
    private readonly Config _config;
    private readonly MongoDbContext _mongoDbContext;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMongoSession _session;

    public LookAroundCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, Config config, MongoDbContext mongoDbContext, IMongoSession session) {
      _playerRepository = playerRepository;
      _mongoDbContext = mongoDbContext;
      _roomRepository = roomRepository;
      _session = session;
      _config = config;
    }

    [Command("olhar")]
    [Aliases("look", "lookaround")]
    [Description("Permite olhar ao seu redor.")]
    [Usage("olhar")]
    public async Task LookAroundCommandAsync(CommandContext ctx) {
      var player = await _mongoDbContext.Players.Find(x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
      if (player == null)
        throw new PlayerNotCreatedException();

      var room = await _mongoDbContext.Rooms.Find(x => x.Id == player.Character.Room.Id).FirstOrDefaultAsync();
      if (room == null) {
        await ctx.ResponderAsync("parece que aconteceu algum erro na matrix!");
        return;
      }

      var embed = new DiscordEmbedBuilder();
      embed.WithColor(DiscordColor.Blue);
      embed.WithTitle(room.Name);
      embed.WithDescription(room.Description);

      var playerLoca = player.Character.Room.Location;

      StringBuilder sf = new StringBuilder();
      var asd = await _mongoDbContext.Rooms.Find(x => x.Location.X >= (playerLoca.X - 160) && x.Location.X <= (playerLoca.X + 160)
                                                  && x.Location.Y >= (playerLoca.Y - 160) && x.Location.Y <= (playerLoca.Y + 160)).ToListAsync();
      int showing = 0;
      foreach (var item in asd) {
        var distance = playerLoca.Distance(item.Location);
        if (distance != 0 && distance <= 165) {
          sf.AppendLine($"{item.Name} - Distancia de {playerLoca.Distance(item.Location):N2} Km.");
          showing++;
        }
      }
      var locations = sf.ToString();
      if (string.IsNullOrWhiteSpace(locations))
        embed.AddField("Locais proximos", "Não tem.");
      else
        embed.AddField("Locais proximos", sf.ToString());

      embed.WithFooter($"Exibindo {showing} locais próximos de {asd.Count}.");

      await ctx.RespondAsync(embed.Build());
    }


    [Command("viajar")]
    [Aliases("v", "travel")]
    [Description("Permite viajar para outro local.")]
    [Usage("viajar [ nome ]")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public async Task TravelCommandAsync(CommandContext ctx, [RemainingText] string roomName) {
      using (var session = await _session.StartSession())
        _res = await session.WithTransactionAsync(async (s, ct) => {
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
      await ctx.RespondAsync(_res);
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
