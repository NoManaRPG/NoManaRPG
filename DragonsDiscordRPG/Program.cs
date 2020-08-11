using DragonsDiscordRPG.Entidades;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDiscordRPG
{
    public class Program
    {
        public static ConfigCore _config;
        static void Main(string[] args) => new Program().RodarOBotAsync().GetAwaiter().GetResult();

        public static string EntrarPasta(string nome)
        {
            StringBuilder raizProjeto = new StringBuilder();
#if DEBUG
            raizProjeto.Append(Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\")));
            raizProjeto.Replace(@"/", @"\");
            return raizProjeto + nome + @"\";
#else
            raizProjeto.Append(Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"../../../../")));
            raizProjeto.Replace(@"\", @"/");
            return raizProjeto + nome + @"/";
#endif
        }

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
#if DEBUG
                Token = _config.TokenTeste,
#else
                Token = _config.Token,
#endif
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
#if DEBUG
                LogLevel = LogLevel.Debug,
#else
                LogLevel = LogLevel.Info,
#endif
                UseInternalLogHandler = true,

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
