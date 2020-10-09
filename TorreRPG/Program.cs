using TorreRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TorreRPG.BancoItens;
using TorreRPG.Config;
using TorreRPG.Services;

namespace TorreRPG
{
    public class Program
    {
        public static ConfigFile configFile;
        static void Main(string[] args) => new Program().RodarBotAsync().GetAwaiter().GetResult();

        public async Task RodarBotAsync()
        {

#if DEBUG
            configFile = ConfigFile.LoadFromFile(StringExtension.EntrarPasta("") + "Config.json");
#else
            configFile = ConfigFile.LoadFromFile("Config.json");
#endif
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
