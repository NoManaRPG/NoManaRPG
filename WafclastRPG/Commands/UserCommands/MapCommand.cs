using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;

namespace WafclastRPG.Commands.UserCommands {
  public class MapCommand : BaseCommandModule {
    public Response Res { private get; set; }
    public Config Config { private get; set; }

    [Command("mapa")]
    [Aliases("map")]
    [Description("Permite ver o mapa para todas os locais disponíveis.")]
    [Cooldown(1, 5, CooldownBucketType.User)]
    [Usage("mapa")]
    public async Task MapCommandAsync(CommandContext ctx) {
      await ctx.TriggerTypingAsync();
      await ctx.RespondAsync(Config.MapUrl);
    }
  }
}
