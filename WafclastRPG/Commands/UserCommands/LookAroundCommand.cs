using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Characters;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands {
  public class LookAroundCommand : BaseCommandModule {
    public DataBase database;

    [Command("olhar")]
    [Aliases("look", "lookaround", "olhar-em-volta")]
    [Description("Permite se localizar olhando o terreno em volta.")]
    [Usage("olhar")]
    public async Task LookAroundCommandAsync(CommandContext ctx) {
      await ctx.TriggerTypingAsync();
      Response response;
      using (var session = await database.StartDatabaseSessionAsync())
        response = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx.User);
          if (player == null)
            return new Response(Messages.AindaNaoCriouPersonagem);

          var character = player.Character;

          var region = await session.FindRegionAsync(character.Region.Id);

          region.Monsters = character.Region.Monsters;
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

          embed.AddField("Saidas", str.ToString());

          return new Response(embed);
        });

      await ctx.ResponderAsync(response.Embed);
    }
  }
}
