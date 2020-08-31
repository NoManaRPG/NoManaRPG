using DragonsDiscordRPG.Comandos;
using DragonsDiscordRPG.Eventos;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace DragonsDiscordRPG
{
    public class ModuloComandos
    {
        public static CommandsNextExtension Comandos { get; private set; }

        public ModuloComandos(CommandsNextConfiguration ccfg, DiscordClient client)
        {
            Comandos = client.UseCommandsNext(ccfg);
            Comandos.CommandExecuted += CommandExecuted.Event;
            Comandos.CommandErrored += CommandErrored.EventAsync;
            // Comandos.SetHelpFormatter<IAjudaComando>();

            Comandos.RegisterCommands<ComandoTeste>();
            Comandos.RegisterCommands<ComandoAdministrativo>();
            Comandos.RegisterCommands<ComandoWiki>();
        }
    }
}
