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
using WafclastRPG.Commands.MercadoGeral;
using WafclastRPG.Commands.RankCommands;
using WafclastRPG.Commands.Skills;
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
            CommandsNext = Client.UseCommandsNext(ccfg);
            CommandsNext.CommandExecuted += CommandExecutedEvent.Event;
            CommandsNext.CommandErrored += CommandErroredEvent.EventAsync;
            Client.Ready += ReadyEvent.Event;

            var botInfo = (BotInfo)CommandsNext.Services.GetService(typeof(BotInfo));
            Client.GuildAvailable += (c, e) => GuildAvailableEvent.Event(c, e, botInfo);
            Client.GuildMemberAdded += (c, e) => GuildMemberAddedEvent.Event(c, e, botInfo);
            Client.GuildMemberRemoved += (c, e) => GuildMemberRemovedEvent.Event(c, e, botInfo);
            Client.MessageCreated += (c, e) => MessageCreatedEvent.Event(c, e, CommandsNext, database);
            Client.ClientErrored += ClientErroredEvent.Event;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30),
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PaginationDeletion = PaginationDeletion.KeepEmojis,
            });

            CommandsNext.SetHelpFormatter<IComandoAjuda>();
            CommandsNext.RegisterCommands<HelpCommand>();
            CommandsNext.RegisterCommands<InfoCommand>();
            CommandsNext.RegisterCommands<ComandoPrefixo>();
            CommandsNext.RegisterCommands<StartCommand>();
            CommandsNext.RegisterCommands<StatusCommand>();
            CommandsNext.RegisterCommands<DatabaseCommands>();
            CommandsNext.RegisterCommands<AttackCommand>();
            CommandsNext.RegisterCommands<InventoryCommand>();
            CommandsNext.RegisterCommands<MoneyRankCommand>();
            CommandsNext.RegisterCommands<LevelRankCommand>();
            CommandsNext.RegisterCommands<EatCommand>();
            CommandsNext.RegisterCommands<CommandsCommand>();
            CommandsNext.RegisterCommands<ExploreCommand>();
            CommandsNext.RegisterCommands<MineCommand>();
            CommandsNext.RegisterCommands<CookCommand>();
            CommandsNext.RegisterCommands<SkillsCommand>();
            CommandsNext.RegisterCommands<GiveItemCommand>();
            CommandsNext.RegisterCommands<BuyCommand>();
            CommandsNext.RegisterCommands<CreateSaleCommand>();
            CommandsNext.RegisterCommands<MyOrdersCommand>();
            CommandsNext.RegisterCommands<StopOrderCommand>();
            CommandsNext.RegisterCommands<AdminCommand>();
            CommandsNext.RegisterCommands<ExamineCommand>();
            CommandsNext.RegisterCommands<AbsorbCommand>();
            CommandsNext.RegisterCommands<SeeCommand>();
            CommandsNext.RegisterCommands<EquipCommand>();
        }
    }
}
