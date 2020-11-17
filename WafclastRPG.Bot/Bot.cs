using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;
using WafclastRPG.Bot.Comandos;
using WafclastRPG.Bot.Comandos.Acao;
using WafclastRPG.Bot.Comandos.Exibir;
using WafclastRPG.Bot.Commands.UserCommands;
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
            this.CommandsNext = Client.UseCommandsNext(ccfg);
            this.CommandsNext.CommandExecuted += CommandExecuted.Event;
            this.CommandsNext.CommandErrored += CommandErrored.EventAsync;
            this.Client.Ready += Ready.Event;
            var botInfo = (BotInfo)CommandsNext.Services.GetService(typeof(BotInfo));
            this.Client.GuildAvailable += (c, e) => GuildAvailable.Event(c, e, botInfo);
            this.Client.GuildMemberAdded += (c, e) => GuildMemberAdded.Event(c, e, botInfo);
            this.Client.GuildMemberRemoved += (c, e) => GuildMemberRemoved.Event(c, e, botInfo);
            this.Client.ClientErrored += ClientErrored.Event;

            this.Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30),
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PaginationDeletion = PaginationDeletion.KeepEmojis,
            });

            this.CommandsNext.SetHelpFormatter<IComandoAjuda>();
            this.CommandsNext.RegisterCommands<ComandoAjuda>();
            this.CommandsNext.RegisterCommands<ComandoStatus>();
            this.CommandsNext.RegisterCommands<ComandoAdministrativo>();
            this.CommandsNext.RegisterCommands<ComandoExplorar>();
            this.CommandsNext.RegisterCommands<ComandoMochila>();
            this.CommandsNext.RegisterCommands<ComandoEquipamentos>();
            this.CommandsNext.RegisterCommands<ComandoEquipar>();
            this.CommandsNext.RegisterCommands<ComandoDesequipar>();
            this.CommandsNext.RegisterCommands<ComandoExaminar>();
            this.CommandsNext.RegisterCommands<ComandoBot>();
            this.CommandsNext.RegisterCommands<ComandoRank>();
            this.CommandsNext.RegisterCommands<ComandoPrefixo>();
            this.CommandsNext.RegisterCommands<ComandoPapear>();
            this.CommandsNext.RegisterCommands<ComandoHabilidade>();
        }
    }
}
