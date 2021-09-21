using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands.CombatCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class ExploreCommand : BaseCommandModule {
    public Response Res { private get; set; }
    public DataBase Data { private get; set; }

    [Command("explorar")]
    [Aliases("ex", "explore")]
    [Description("Permite explorar uma região, podendo encontrar monstros.")]
    [Usage("explorar")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    public async Task ExploreCommandAsync(CommandContext ctx) {
      using (var session = await Data.StartDatabaseSessionAsync())
        Res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx);

          var character = player.Character;
          character.Room = await session.FindRoomAsync(character.Room.Id);

          if (character.Room.Monster == null) {
            return new Response("você procura monstros para atacar.. mas parece que não você não encontrará nada aqui.");
          }

          await player.SaveAsync();

          return new Response($"você encontrou **[{character.Room.Monster.Mention}]!**");
        });
      await ctx.ResponderAsync(Res);
    }
  }
}
