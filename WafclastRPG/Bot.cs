// This file is part of the WafclastRPG project.

using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using WafclastRPG.Commands;
using WafclastRPG.DiscordEvents;

namespace WafclastRPG
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension CommandsNext { get; private set; }

        public Bot(DiscordConfiguration discordConfiguration)
        {
            this.Client = new DiscordClient(discordConfiguration);
        }

        public Task ConectarAsync() => this.Client.ConnectAsync();

        public void ModuleCommand(CommandsNextConfiguration ccfg)
        {
            this.CommandsNext = this.Client.UseCommandsNext(ccfg);
            this.CommandsNext.CommandExecuted += CommandExecutedEvent.Event;
            this.CommandsNext.CommandErrored += CommandErroredEvent.EventAsync;
            this.Client.Ready += ReadyEvent.Event;

            this.Client.GuildAvailable += (c, e) => GuildAvailableEvent.Event(c, e);
            this.Client.ClientErrored += ClientErroredEvent.Event;

            this.Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2),
                PollBehaviour = PollBehaviour.KeepEmojis,
                PaginationBehaviour = PaginationBehaviour.Ignore,
                PaginationDeletion = PaginationDeletion.KeepEmojis,
            });

            this.Client.MessageCreated += async (s, e) =>
            {
                if (e.Message.Content.ToLower().StartsWith("ping"))
                    await e.Message.RespondAsync("pong!");
            };

            //this.CommandsNext.SetHelpFormatter<IHelpCommand>();
            //this.CommandsNext.RegisterCommands(Assembly.GetExecutingAssembly());
        }
    }
}
