using TorreRPG.Extensoes;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TorreRPG.Eventos
{
    public static class CommandExecuted
    {
        public static Task Event(CommandsNextExtension cne, CommandExecutionEventArgs e)
        {
            cne.Client.Logger.Log(LogLevel.Information, $"({e.Context.Guild.Id}) " +
                $"{e.Context.Guild.Name.RemoverAcentos()}", $"({e.Context.User.Id}) " +
                $"{e.Context.User.Username.RemoverAcentos()} executou '{e.Command.QualifiedName}'", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
