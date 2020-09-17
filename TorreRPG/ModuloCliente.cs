using DSharpPlus;
using TorreRPG.Eventos;
using TorreRPG.Extensoes;

namespace TorreRPG
{
    public class ModuloCliente
    {
        public static DiscordClient Client { get; private set; }
        public static BotInfo Bot { get; private set; }
        public ModuloCliente(DiscordConfiguration discordConfiguration)
        {
            Bot = BotInfo.LoadFromFile(StringExtension.EntrarPasta("") + "BotInfo.json");
            if (Bot == null) Bot = new BotInfo();
#if DEBUG
            Bot.VersaoRevisao++;
            Bot.SaveToFile(StringExtension.EntrarPasta("") + "BotInfo.json");
#endif

            Client = new DiscordClient(discordConfiguration);
            Client.Ready += (e) => Ready.Event(e, Client);
            Client.GuildAvailable += (e) => GuildAvailable.Event(e, Bot);
            Client.ClientErrored += ClientErrored.Event;
            Client.MessageCreated += MessageCreated.EventAsync;
        }
    }
}
