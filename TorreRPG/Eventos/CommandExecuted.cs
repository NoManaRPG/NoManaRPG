using TorreRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;

namespace TorreRPG.Eventos
{
    public static class CommandExecuted
    {
        public static Task Event(CommandExecutionEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, $"({e.Context.Guild.Id}) " +
                $"{e.Context.Guild.Name.RemoverAcentos()}", $"({e.Context.User.Id}) " +
                $"{e.Context.User.Username.RemoverAcentos()} executou '{e.Command.QualifiedName}'", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
