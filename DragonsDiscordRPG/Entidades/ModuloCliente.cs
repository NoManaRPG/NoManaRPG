using DragonsDiscordRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "ZAYN", "Cliente está pronto para processar eventos.", DateTime.Now);
#if DEBUG
            string projetoRaiz = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")) + "BotCore.json";
#else
            string projetoRaiz = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"../../../../")) + "BotCore.json";
#endif
            Bot = BotCore.LoadFromFile(projetoRaiz);
            if (Bot == null)
                Bot = new BotCore();
#if DEBUG
            Bot.VersaoRevisao++;
            Bot.SaveToFile(projetoRaiz);
#endif
            Client.UpdateStatusAsync(new DiscordActivity($"z!ajuda", ActivityType.Playing), UserStatus.Online);
            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "Zayn", $"Guilda {e.Guild.Name.RemoverAcentos()}", DateTime.Now);
            Bot.QuantidadeServidores++;
            Bot.QuantidadeMembros += e.Guild.MemberCount;
            Bot.QuantidadeCanais += e.Guild.Channels.Count;
            return Task.CompletedTask;
        }

        private Task Client_ClientError(ClientErrorEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "Zayn", $"Um erro aconteceu: {e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
