// This file is part of NoManaRPG project.

using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;
using NoManaRPG.Exceptions;

namespace NoManaRPG.DiscordEvents;

public static class SlashCommandErrorEvent
{
    public static async Task EventAsync(SlashCommandsExtension sce, SlashCommandErrorEventArgs e)
    {
        InteractionContext ctx = e.Context;
        switch (e.Exception)
        {
            case PlayerNotCreatedException pnce:
                await ctx.CreateResponseAsync($"{ctx.User.Mention}, você {pnce.Message}", true);
                break;
            case SlashExecutionChecksFailedException secfe:
                if (secfe.FailedChecks.FirstOrDefault(x => x is SlashCooldownAttribute) is SlashCooldownAttribute sca)
                {
                    await ctx.CreateResponseAsync($"{ctx.Member.Mention}, " +
                        $"{Formatter.Timestamp(sca.GetRemainingCooldown(ctx))} " +
                        $"este comando ficará disponível!", true);
                    break;
                }
                break;
            default:

                break;
        }
    }
}
