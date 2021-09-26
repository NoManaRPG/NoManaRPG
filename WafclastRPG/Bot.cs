using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;
using WafclastRPG.Commands;
using WafclastRPG.DiscordEvents;

namespace WafclastRPG {
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

      Client.GuildAvailable += (c, e) => GuildAvailableEvent.Event(c, e);
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
