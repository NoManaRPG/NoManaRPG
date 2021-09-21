using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;
using WafclastRPG.Commands;
using WafclastRPG.Commands.AdminCommands;
using WafclastRPG.Commands.UserCommands;
using WafclastRPG.Commands.UserCommands.CombatCommands;
using WafclastRPG.DataBases;
using WafclastRPG.DiscordEvents;

namespace WafclastRPG {
  public class BotInfo {
    public int Membros;
    public int Guildas;
    public DateTime TempoAtivo { get; set; } = DateTime.Now;
  }

  public class Bot {
    public DiscordClient Client { get; private set; }
    public CommandsNextExtension CommandsNext { get; private set; }

    public Bot(DiscordConfiguration discordConfiguration)
        => Client = new DiscordClient(discordConfiguration);

    public Task ConectarAsync() => Client.ConnectAsync();

    public void ModuleCommand(CommandsNextConfiguration ccfg) {
      CommandsNext = Client.UseCommandsNext(ccfg);
      CommandsNext.CommandExecuted += CommandExecutedEvent.Event;
      CommandsNext.CommandErrored += CommandErroredEvent.EventAsync;
      Client.Ready += ReadyEvent.Event;

      var botInfo = (BotInfo) CommandsNext.Services.GetService(typeof(BotInfo));
      Client.GuildAvailable += (c, e) => GuildAvailableEvent.Event(c, e, botInfo);
      Client.GuildMemberAdded += (c, e) => GuildMemberAddedEvent.Event(c, e, botInfo);
      Client.GuildMemberRemoved += (c, e) => GuildMemberRemovedEvent.Event(c, e, botInfo);
      Client.MessageCreated += (c, e) => MessageCreatedEvent.Event(c, e, CommandsNext);
      Client.ClientErrored += ClientErroredEvent.Event;

      Client.UseInteractivity(new InteractivityConfiguration {
        Timeout = TimeSpan.FromSeconds(30),
        PollBehaviour = PollBehaviour.KeepEmojis,
        PaginationBehaviour = PaginationBehaviour.Ignore,
        PaginationDeletion = PaginationDeletion.KeepEmojis,
      });

      CommandsNext.SetHelpFormatter<IHelpCommand>();
      CommandsNext.RegisterCommands(Assembly.GetExecutingAssembly());
    }
  }
}
