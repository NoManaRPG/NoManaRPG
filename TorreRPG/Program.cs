using TorreRPG.Extensoes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TorreRPG
{
    public class Program
    {
        public static ConfigFile configFile;
        static void Main(string[] args) => new Program().RodarOBotAsync().GetAwaiter().GetResult();

        public async Task RodarOBotAsync()
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

            ModuloCliente cliente = new ModuloCliente(new DiscordConfiguration
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
                LogLevel = LogLevel.Info,
#endif
            });
            ModuloComandos todosOsComandos = new ModuloComandos(new CommandsNextConfiguration
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
            }, ModuloCliente.Client);

            ModuloBanco.Conectar();

            await ModuloCliente.Client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
