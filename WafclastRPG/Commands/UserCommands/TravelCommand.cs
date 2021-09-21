using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;
namespace WafclastRPG.Commands.UserCommands {
  public class TravelCommand : BaseCommandModule {
    public Response Res { private get; set; }
    public DataBase Data { private get; set; }

    [Command("viajar")]
    [Aliases("v", "travel")]
    [Description("Permite viajar para outro local.")]
    [Usage("viajar [ nome ]")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public async Task TravelCommandAsync(CommandContext ctx, [RemainingText] string roomName) {
      using (var session = await Data.StartDatabaseSessionAsync())
        Res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx);

          var character = player.Character;
          Room room = null;

          if (string.IsNullOrWhiteSpace(roomName)) {
            room = await session.FindRoomAsync(ctx.Channel.Id);
            if (room == null)
              return new Response("você foi para algum lugar, talvez alguns passos a frente.");
          } else {
            room = await session.FindRoomAsync(roomName);
            if (room == null)
              return new Response("você tenta procurar no mapa o lugar, mas não encontra! Como você chegaria em um lugar em que você não conhece?!");
          }

          if (room.Location.Distance(character.Room.Location) > 161)
            return new Response("parece ser um caminho muito longe! Melhor tentar algo mais próximo.");
          if (room == player.Character.Room)
            return new Response("como é bom estar no lugar que você sempre quis...");

          room.Monster = null;
          character.Room = room;
          await player.SaveAsync();

          return new Response($"você chegou em: **[{room.Name}]!**");
        });
      await ctx.ResponderAsync(Res);
    }
  }
}
