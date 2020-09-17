using TorreRPG.Comandos;
using TorreRPG.Comandos.Acao;
using TorreRPG.Comandos.Exibir;
using TorreRPG.Eventos;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace TorreRPG
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
            Comandos.RegisterCommands<ComandoStatus>();
            Comandos.RegisterCommands<ComandoAdministrativo>();
            Comandos.RegisterCommands<ComandoWiki>();
            Comandos.RegisterCommands<ComandoCriarPersonagem>();
            Comandos.RegisterCommands<ComandoAtacar>();
            Comandos.RegisterCommands<ComandoDescer>();
            Comandos.RegisterCommands<ComandoSubir>();
            Comandos.RegisterCommands<ComandoExplorar>();
            Comandos.RegisterCommands<ComandoOlhar>();
            Comandos.RegisterCommands<ComandoUsarPocao>();
            Comandos.RegisterCommands<ComandoPegar>();
            Comandos.RegisterCommands<ComandoMochila>();
            Comandos.RegisterCommands<ComandoEquipamentos>();
            Comandos.RegisterCommands<ComandoEquipar>();
        }
    }
}
