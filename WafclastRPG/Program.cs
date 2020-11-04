using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using WafclastRPG.Bot.Config;
using WafclastRPG.Game;

namespace WafclastRPG.Bot
{
    public class Program
    {
        public ConfigFile ConfigFile { get; private set; }
        public BotInfo BotInfo { get; private set; }
        static void Main(string[] args) => new Program().RodarBotAsync().GetAwaiter().GetResult();

        public async Task RodarBotAsync()
        {
            ConfigFile = ConfigFile.LoadFromFile("Config.json");
            if (ConfigFile == null)
            {
                Console.WriteLine("O arquivo config.json não existe!");
                Console.WriteLine("Coloque as informações necessarias no arquivo gerado!");
                Console.WriteLine("Aperte qualquer botão para sair...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            #region Configs
#if DEBUG
            var token = ConfigFile.TokenTeste;
            var logLevel = LogLevel.Debug;
            var prefix = new string[1] { ConfigFile.PrefixTeste };
#else
            var token = ConfigFile.Token;
            var logLevel = LogLevel.Information;
            var prefix = new string[1] { ConfigFile.Prefix };
#endif
            #endregion

            Bot bot = new Bot(new DiscordConfiguration
            {
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
                Token = token,
                MinimumLogLevel = logLevel,
            });

            BotInfo = BotInfo.LoadFromFile("BotInfo.json");
            BotInfo.VersaoRevisao++;
            BotInfo.SaveToFile("BotInfo.json");

            var services = new ServiceCollection()
                .AddSingleton<Banco>()
                .AddSingleton<ConfigFile>()
                .AddSingleton<BotInfo>()
                .BuildServiceProvider();

            bot.ModuloComando(new CommandsNextConfiguration
            {
                StringPrefixes = prefix,
                EnableDms = false,
                CaseSensitive = false,
                EnableDefaultHelp = false,
                EnableMentionPrefix = true,
                IgnoreExtraArguments = true,
                Services = services,
            });

            await bot.ConectarAsync();
            await Task.Delay(-1);
        }
    }
}
