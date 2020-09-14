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

            Comandos.SetHelpFormatter<IAjudaComando>();

            Comandos.RegisterCommands<ComandoAjuda>();
            Comandos.RegisterCommands<ComandoTeste>();
            Comandos.RegisterCommands<ComandoStatus>();
            Comandos.RegisterCommands<ComandoAdministrativo>();
            Comandos.RegisterCommands<ComandoWiki>();
            Comandos.RegisterCommands<ComandoCriarPersonagem>();
            Comandos.RegisterCommands<ComandoAtacar>();
            Comandos.RegisterCommands<ComandoDescer>();
            Comandos.RegisterCommands<ComandoSubir>();
            Comandos.RegisterCommands<ComandoExplorar>();
            Comandos.RegisterCommands<ComandoOlhar>();
            Comandos.RegisterCommands<ComandoUsar>();
            Comandos.RegisterCommands<ComandoPegar>();
            Comandos.RegisterCommands<ComandoMochila>();
            Comandos.RegisterCommands<ComandoEquipamentos>();
            Comandos.RegisterCommands<ComandoEquipar>();
        }
    }
}
