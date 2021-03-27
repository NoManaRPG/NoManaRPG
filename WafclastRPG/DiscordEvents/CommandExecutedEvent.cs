using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WafclastRPG.Extensions;

namespace WafclastRPG.DiscordEvents
{
    public static class CommandExecutedEvent
    {
        public static Task Event(CommandsNextExtension cne, CommandExecutionEventArgs e)
        {
            cne.Client.Logger.LogInformation(new EventId(600, "Comando exec"), $"{e.Context.Guild.Name.RemoverAcentos()} - {e.Context.User.Id} executou '{e.Command.QualifiedName}'.", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
