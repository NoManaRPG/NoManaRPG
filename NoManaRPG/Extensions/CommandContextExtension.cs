// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using NoManaRPG.Database.Response;

namespace NoManaRPG.Extensions;

public static class CommandContextExtension
{
    public static Task<DiscordMessage> RespondAsync(this CommandContext ctx, DiscordEmbedBuilder embed)
      => ctx.RespondAsync(ctx.User.Mention, embed: embed.Build());
    public static Task<DiscordMessage> RespondAsync(this CommandContext ctx, string mensagem)
      => ctx.RespondAsync($"{ctx.User.Mention}, {mensagem}");

    public static Task<DiscordMessage> RespondAsync(this CommandContext ctx, IResponse response)
      => response switch
      {
          StringResponse res => RespondAsync(ctx, res.Response),
          EmbedResponse res => RespondAsync(ctx, res.Response),
          _ => throw new System.Exception("Resposta não definida!"),
      };
}
