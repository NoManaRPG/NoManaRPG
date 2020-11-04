using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using WafclastRPG.Bot.Comandos;
using WafclastRPG.Bot.Comandos.Acao;
using WafclastRPG.Bot.Comandos.Exibir;
using WafclastRPG.Bot.Config;
using WafclastRPG.Bot.Eventos;

namespace WafclastRPG.Bot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension CommandsNext { get; private set; }

        public Bot(DiscordConfiguration discordConfiguration)
        {
            Client = new DiscordClient(discordConfiguration);
        }

        public Task ConectarAsync() => Client.ConnectAsync();

        public void ModuloComando(CommandsNextConfiguration ccfg)
        {
            CommandsNext = Client.UseCommandsNext(ccfg);
            CommandsNext.CommandExecuted += CommandExecuted.Event;
            CommandsNext.CommandErrored += CommandErrored.EventAsync;
            Client.Ready += Ready.Event;
            var botInfo = (BotInfo)CommandsNext.Services.GetService(typeof(BotInfo));
            Client.GuildAvailable += (c, e) => GuildAvailable.Event(c, e, botInfo);
            Client.GuildMemberAdded += (c, e) => GuildMemberAdded.Event(c, e, botInfo);
            Client.GuildMemberRemoved += (c, e) => GuildMemberRemoved.Event(c, e, botInfo);
            Client.ClientErrored += ClientErrored.Event;

            CommandsNext.SetHelpFormatter<IAjudaComando>();

            CommandsNext.RegisterCommands<ComandoAjuda>();
            CommandsNext.RegisterCommands<ComandoStatus>();
            CommandsNext.RegisterCommands<ComandoAdministrativo>();
            CommandsNext.RegisterCommands<ComandoCriarPersonagem>();
            CommandsNext.RegisterCommands<ComandoAtacar>();
            CommandsNext.RegisterCommands<ComandoMochila>();
            CommandsNext.RegisterCommands<ComandoEquipamentos>();
            CommandsNext.RegisterCommands<ComandoEquipar>();
            CommandsNext.RegisterCommands<ComandoDesequipar>();
            CommandsNext.RegisterCommands<ComandoExaminar>();
            CommandsNext.RegisterCommands<ComandoVender>();
            CommandsNext.RegisterCommands<ComandoBot>();
        }
    }
}
