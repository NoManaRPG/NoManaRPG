using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;
using WafclastRPG.Bot.Comandos.Acao;
using WafclastRPG.Bot.Comandos.Exibir;
using WafclastRPG.Bot.Commands.AdminCommands;
using WafclastRPG.Bot.Commands.GeneralCommands;
using WafclastRPG.Bot.Commands.UserCommands;
using WafclastRPG.Bot.DiscordEvents;

namespace WafclastRPG.Bot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension CommandsNext { get; private set; }

        public Bot(DiscordConfiguration discordConfiguration)
            => Client = new DiscordClient(discordConfiguration);

        public Task ConectarAsync() => Client.ConnectAsync();

        public void ModuleCommand(CommandsNextConfiguration ccfg, Database database)
        {
            this.CommandsNext = Client.UseCommandsNext(ccfg);
            this.CommandsNext.CommandExecuted += CommandExecutedEvent.Event;
            this.CommandsNext.CommandErrored += CommandErroredEvent.EventAsync;
            this.Client.Ready += ReadyEvent.Event;

            var botInfo = (BotInfo)CommandsNext.Services.GetService(typeof(BotInfo));
            this.Client.GuildAvailable += (c, e) => GuildAvailableEvent.Event(c, e, botInfo);
            this.Client.GuildMemberAdded += (c, e) => GuildMemberAddedEvent.Event(c, e, botInfo);
            this.Client.GuildMemberRemoved += (c, e) => GuildMemberRemovedEvent.Event(c, e, botInfo);
            this.Client.MessageCreated += (c, e) => MessageCreatedEvent.Event(c, e, CommandsNext, database);
            this.Client.MessageDeleted += MessageDeletedEvent.Event;
            this.Client.MessageUpdated += MessageUpdatedEvent.Event;
            this.Client.ClientErrored += ClientErroredEvent.Event;

            this.Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30),
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PaginationDeletion = PaginationDeletion.KeepEmojis,
            });

            this.CommandsNext.SetHelpFormatter<IComandoAjuda>();
            this.CommandsNext.RegisterCommands<HelpCommand>();
            this.CommandsNext.RegisterCommands<ComandoEquipamentos>();
            this.CommandsNext.RegisterCommands<InfoCommand>();
            this.CommandsNext.RegisterCommands<ComandoPrefixo>();
            this.CommandsNext.RegisterCommands<StartCommand>();
            this.CommandsNext.RegisterCommands<TestCommands>();
            this.CommandsNext.RegisterCommands<StatusCommand>();
            this.CommandsNext.RegisterCommands<DatabaseCommands>();
            this.CommandsNext.RegisterCommands<AttackCommand>();
            this.CommandsNext.RegisterCommands<TravelCommand>();
            this.CommandsNext.RegisterCommands<LookCommand>();
        }
    }
}
