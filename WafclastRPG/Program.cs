using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using WafclastRPG.Game.BancoItens;
using WafclastRPG.Game.Config;
using WafclastRPG.Game.Services;

namespace WafclastRPG.Game
{
    public class Program
    {
        public static ConfigFile configFile;
        static void Main(string[] args) => new Program().RodarBotAsync().GetAwaiter().GetResult();

        public async Task RodarBotAsync()
        {
            configFile = ConfigFile.LoadFromFile("Config.json");
            if (configFile == null)
            {
                Console.WriteLine("O arquivo config.json não existe!");
                Console.WriteLine("Coloque as informações necessarias no arquivo gerado!");
                Console.WriteLine("Aperte qualquer botão para sair...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Bot bot = new Bot(new DiscordConfiguration
            {
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
#if DEBUG
                Token = configFile.TokenTeste,
                MinimumLogLevel = LogLevel.Debug,
#else
                Token = configFile.Token,
                MinimumLogLevel = LogLevel.Information,
#endif
            });

            // Dependency Injection
            var services = new ServiceCollection()
                .AddSingleton<Banco>()
                .BuildServiceProvider();

            bot.ModuloComando(new CommandsNextConfiguration
            {
#if DEBUG
                StringPrefixes = new string[1] { configFile.PrefixTeste },
#else
                StringPrefixes = new string[1] { configFile.Prefix },
#endif
                EnableDms = false,
                CaseSensitive = false,
                EnableDefaultHelp = false,
                EnableMentionPrefix = true,
                IgnoreExtraArguments = true,
                Services = services,
            });

            RPMetadata.Carregar();

            await Bot.Cliente.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
