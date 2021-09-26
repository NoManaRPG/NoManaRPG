using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Commands;

namespace WafclastRPG.Extensions {
  public static class CommandContextExtension {
    public static Task<DiscordMessage> ResponderAsync(this CommandContext ctx, string mensagem)
        => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}");
    public static async Task<DiscordMessage> RespondAsync(this CommandContext ctx, Response response) {
      if (response.Message != null)
        return await ctx.ResponderAsync(response.Message);
      else
        return await ctx.ResponderAsync(response.Embed);
    }

    public static Task<DiscordMessage> ResponderAsync(this CommandContext ctx, string mensagem, DiscordEmbed embed)
        => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}", embed);

    public static Task<DiscordMessage> ResponderAsync(this CommandContext ctx, DiscordEmbed embed)
        => ctx.RespondAsync(ctx.User.Mention, embed: embed);

    public static T GetService<T>(this CommandContext ctx) where T : class {
      return (T) ctx.Services.GetService(typeof(T));
    }
  }
}
