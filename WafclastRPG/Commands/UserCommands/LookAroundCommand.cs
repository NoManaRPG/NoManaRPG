using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Wafclast;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WafclastRPG.Exceptions;
using WafclastRPG.Extensions;
using DSharpPlus.Interactivity.Extensions;
using System;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class LookAroundCommand : BaseCommandModule {
    public Response Res { private get; set; }
    public DataBase Data { private get; set; }

    [Command("olhar")]
    [Aliases("look", "lookaround")]
    [Description("Permite olhar ao seu redor e outro local.")]
    [Usage("olhar [ nome ]")]
    public async Task LookAroundCommandAsync(CommandContext ctx, [RemainingText] string roomName) {

      var player = await Data.CollectionPlayers.Find(x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
      if (player == null)
        throw new PlayerNotCreatedException();

      Room room = null;

      if (string.IsNullOrWhiteSpace(roomName)) {
        room = await Data.CollectionRooms.Find(x => x.Id == player.Character.Room.Id).FirstOrDefaultAsync();
        if (room == null) {
          await ctx.ResponderAsync("parece que aconteceu algum erro na matrix!");
          return;
        }
      } else {
        room = await Data.CollectionRooms.Find(x => x.Name == roomName, new FindOptions { Collation = new Collation("pt", false, strength: CollationStrength.Primary) }).FirstOrDefaultAsync();
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

          var travel = new TravelCommand();
          travel.Data = Data;
          travel.Res = Res;
          await travel.TravelCommandAsync(ctx, room.Name);
        }
    }
  }
}
