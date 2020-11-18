using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using WafclastRPG.Bot.Config;
using WafclastRPG.Game;
using DSharpPlus.Entities;

namespace WafclastRPG.Bot
{
    public class Program
    {
        public ConfigFile ConfigFile { get; private set; }
        public BotInfo BotInfo { get; private set; }
        public Banco Banco { get; private set; } = new Banco();

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
            ConfigFile.Prefix = ConfigFile.PrefixTeste;
            var logLevel = LogLevel.Debug;
#else
            var token = ConfigFile.Token;
            var logLevel = LogLevel.Information;
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
                .AddSingleton(this.Banco)
                .AddSingleton(this.ConfigFile)
                .AddSingleton(this.BotInfo)
                .AddSingleton<BotMathematics>()
                .BuildServiceProvider();

            bot.ModuloComando(new CommandsNextConfiguration
            {
                PrefixResolver = this.ResolvePrefixAsync,
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

        private async Task<int> ResolvePrefixAsync(DiscordMessage msg)
        {
            var gld = msg.Channel.Guild;
            if (gld == null)
                return await Task.FromResult(-1);
#if DEBUG
            if (Banco.IsExecutingInteractivity(msg.Author.Id))
                return await Task.FromResult(-1);
            var prefix = await Banco.GetServerPrefixAsync(gld.Id, ConfigFile.PrefixTeste);
            var pfixLocation = msg.GetStringPrefixLength(prefix);
#else
            var prefix = await Banco.GetServerPrefixAsync(gld.Id, ConfigFile.Prefix);
            var pfixLocation = msg.GetStringPrefixLength(prefix);
            if (pfixLocation == -1)
                pfixLocation = msg.GetStringPrefixLength(ConfigFile.Prefix.ToLower());
            if (pfixLocation == -1)
                pfixLocation = msg.GetStringPrefixLength(ConfigFile.Prefix.ToUpper());
#endif
            return await Task.FromResult(pfixLocation);
        }
    }
}
