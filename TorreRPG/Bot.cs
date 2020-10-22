using DSharpPlus;
using DSharpPlus.CommandsNext;
using TorreRPG.Comandos;
using TorreRPG.Comandos.Acao;
using TorreRPG.Comandos.Exibir;
using TorreRPG.Config;
using TorreRPG.Eventos;
using TorreRPG.Extensoes;

namespace TorreRPG
{
    public class Bot
    {
        public static DiscordClient Cliente { get; private set; }
        public CommandsNextExtension ComandosNext { get; private set; }
        public BotInfo BotInfo { get; private set; }

        public Bot(DiscordConfiguration discordConfiguration)
        {
            BotInfo = BotInfo.LoadFromFile(StringExtension.EntrarPasta("") + "BotInfo.json");
            if (BotInfo == null) BotInfo = new BotInfo();
#if DEBUG
            BotInfo.VersaoRevisao++;
            BotInfo.SaveToFile(StringExtension.EntrarPasta("") + "BotInfo.json");
#endif

            Cliente = new DiscordClient(discordConfiguration);
            Cliente.Ready += Ready.Event;
            Cliente.GuildAvailable += (c, e) => GuildAvailable.Event(c, e, BotInfo);
            Cliente.ClientErrored += ClientErrored.Event;
        }

        public void ModuloComando(CommandsNextConfiguration ccfg)
        {
            ComandosNext = Cliente.UseCommandsNext(ccfg);
            ComandosNext.CommandExecuted += CommandExecuted.Event;
            ComandosNext.CommandErrored += CommandErrored.EventAsync;

            ComandosNext.SetHelpFormatter<IAjudaComando>();

            ComandosNext.RegisterCommands<ComandoAjuda>();
            ComandosNext.RegisterCommands<ComandoStatus>();
            ComandosNext.RegisterCommands<ComandoAdministrativo>();
            ComandosNext.RegisterCommands<ComandoWiki>();
            ComandosNext.RegisterCommands<ComandoCriarPersonagem>();
            ComandosNext.RegisterCommands<ComandoAtacar>();
            ComandosNext.RegisterCommands<ComandoDescer>();
            ComandosNext.RegisterCommands<ComandoSubir>();
            ComandosNext.RegisterCommands<ComandoExplorar>();
            ComandosNext.RegisterCommands<ComandoZona>();
            ComandosNext.RegisterCommands<ComandoUsarPocao>();
            ComandosNext.RegisterCommands<ComandoPegar>();
            ComandosNext.RegisterCommands<ComandoMochila>();
            ComandosNext.RegisterCommands<ComandoEquipamentos>();
            ComandosNext.RegisterCommands<ComandoEquipar>();
            ComandosNext.RegisterCommands<ComandoDesequipar>();
            ComandosNext.RegisterCommands<ComandoSetImage>();
            ComandosNext.RegisterCommands<ComandoExaminar>();
            ComandosNext.RegisterCommands<ComandoChao>();
            ComandosNext.RegisterCommands<ComandoMonstros>();
            ComandosNext.RegisterCommands<ComandoVender>();
            ComandosNext.RegisterCommands<ComandoLoja>();
            ComandosNext.RegisterCommands<ComandoComprar>();
        }
    }
}
