using DragonsDiscordRPG.Entidades;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using static DragonsDiscordRPG.Entidades.Extras;

namespace DragonsDiscordRPG
{
    public class Program
    {
        public static ConfigCore _config;
        static void Main(string[] args) => new Program().RodarOBotAsync().GetAwaiter().GetResult();

        public async Task RodarOBotAsync()
        {
            _config = ConfigCore.LoadFromFile(EntrarPasta("") + "config.json");
            if (_config == null)
            {
                Console.WriteLine("O arquivo config.json não existe!");
                Console.WriteLine("Coloque as informações necessarias no arquivo gerado!");
                Console.WriteLine("Aperte qualquer botão para sair...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            DiscordConfiguration cfg = new DiscordConfiguration
            {
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
                UseInternalLogHandler = true,
#if DEBUG
                Token = _config.TokenTeste,
                LogLevel = LogLevel.Debug,
#else
                Token = _config.Token,
                LogLevel = LogLevel.Info,
#endif
            };
            ModuloCliente cliente = new ModuloCliente(cfg);

            string[] prefix = new string[1];
#if DEBUG
            prefix[0] = _config.PrefixTeste;
#else
            prefix[0] = _config.Prefix;
#endif
            ModuloComandos todosOsComandos = new ModuloComandos(new CommandsNextConfiguration
            {
                StringPrefixes = prefix,
                EnableDms = false,
                CaseSensitive = false,
                EnableDefaultHelp = false,
                EnableMentionPrefix = true,
                IgnoreExtraArguments = true,
            }, ModuloCliente.Client);

            new ModuloBanco();

            await ModuloCliente.Client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
