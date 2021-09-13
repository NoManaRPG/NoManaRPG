using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class LookAroundCommand : BaseCommandModule {
    public Response Res { private get; set; }
    public DataBase Data { private get; set; }

    [Command("olhar")]
    [Aliases("look", "lookaround", "olhar-em-volta")]
    [Description("Permite se localizar olhando o terreno em volta.")]
    [Usage("olhar")]
    public async Task LookAroundCommandAsync(CommandContext ctx) {
      using (var session = await Data.StartDatabaseSessionAsync())
        Res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx);

          var character = player.Character;

          var region = await session.FindRegionAsync(character.Region.Id);

          region.Monster = character.Region.Monster;
          region.MonsterAttackSpeedPoints = character.Region.MonsterAttackSpeedPoints;
          region.PlayerAttackSpeedPoints = character.Region.PlayerAttackSpeedPoints;
          region.TotalAttackSpeedPoints = character.Region.TotalAttackSpeedPoints;

          character.Region = region;

          var embed = new DiscordEmbedBuilder();
          embed.WithColor(DiscordColor.Green);
          embed.WithTitle($"{region.Name} [{region.Id}]");
          embed.WithDescription(region.Description);

          var str = new StringBuilder();
          foreach (var item in region.Exits) {
            str.AppendLine($"{item.Key} - {item.Value}");
          }

          if (string.IsNullOrWhiteSpace(str.ToString()))
            str.AppendLine("Parece que não existe saidas!");

          embed.AddField("Saidas", str.ToString());

          return new Response(embed);
        });

      await ctx.ResponderAsync(Res);
    }
  }
}
