using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;
using WafclastRPG.Comandos.Acao;
using WafclastRPG.Commands.AdminCommands;
using WafclastRPG.Commands.GeneralCommands;
using WafclastRPG.Commands.RankCommands;
using WafclastRPG.Commands.UserCommands;
using WafclastRPG.DataBases;
using WafclastRPG.DiscordEvents;

namespace WafclastRPG
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension CommandsNext { get; private set; }

        public Bot(DiscordConfiguration discordConfiguration)
            => Client = new DiscordClient(discordConfiguration);

        public Task ConectarAsync() => Client.ConnectAsync();

        public void ModuleCommand(CommandsNextConfiguration ccfg, DataBase database)
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
            this.CommandsNext.RegisterCommands<LookCommand>();
            this.CommandsNext.RegisterCommands<InventoryCommand>();
            this.CommandsNext.RegisterCommands<MoneyRankCommand>();
            this.CommandsNext.RegisterCommands<LevelRankCommand>();
            this.CommandsNext.RegisterCommands<EatCommand>();
            this.CommandsNext.RegisterCommands<AttributesCommand>();
            this.CommandsNext.RegisterCommands<ShopCommand>();
            this.CommandsNext.RegisterCommands<EvolveGroupCommands>();
            this.CommandsNext.RegisterCommands<CommandsCommand>();
            this.CommandsNext.RegisterCommands<ExploreCommand>();
            this.CommandsNext.RegisterCommands<MineCommand>();
            this.CommandsNext.RegisterCommands<CookCommand>();
            this.CommandsNext.RegisterCommands<SkillsCommand>();
            this.CommandsNext.RegisterCommands<MoveUpCommand>();
            this.CommandsNext.RegisterCommands<MoveDownCommand>();
        }
    }
}
