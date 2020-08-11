using DragonsDiscordRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using static DragonsDiscordRPG.Entidades.Extras;

namespace DragonsDiscordRPG.Entidades
{
    public class ModuloCliente
    {
        public static DiscordClient Client { get; private set; }
        public static BotCore Bot { get; private set; }
        public ModuloCliente(DiscordConfiguration discordConfiguration)
        {
            Client = new DiscordClient(discordConfiguration);
            Client.Ready += Client_Ready;
            Client.GuildAvailable += Client_GuildAvailable;
            Client.ClientErrored += Client_ClientError;
        }

        private Task Client_Ready(ReadyEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Dragon", "Cliente está pronto.", DateTime.Now);
            Bot = BotCore.LoadFromFile(EntrarPasta("") + "BotCore.json");
            if (Bot == null)
                Bot = new BotCore();
            Client.UpdateStatusAsync(new DiscordActivity($"!ajuda", ActivityType.Playing), UserStatus.Online);
#if DEBUG
            Bot.VersaoRevisao++;
            Bot.SaveToFile(EntrarPasta("") + "BotCore.json");
#endif
            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Dragon", $"Guilda {e.Guild.Name.RemoverAcentos()}", DateTime.Now);
            Bot.QuantidadeMembros += e.Guild.MemberCount;
            return Task.CompletedTask;
        }

        private Task Client_ClientError(ClientErrorEventArgs e)
        {
            string erro = $"Um erro aconteceu no client: {e.Exception.GetType()}: {e.Exception.Message}";
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "Dragon", erro, DateTime.Now);
            DiscordChannel channel = e.Client.GetChannelAsync(742778666509008956).Result;
            e.Client.SendMessageAsync(channel, erro);
            return Task.CompletedTask;
        }
    }
}
