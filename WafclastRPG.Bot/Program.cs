using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using WafclastRPG.Bot.Config;
using WafclastRPG.Game;
using WafclastRPG.Game.Metadata;
using DSharpPlus.Entities;

namespace WafclastRPG.Bot
{
    public class Program
    {
        public ConfigFile ConfigFile { get; private set; }
        public BotInfo BotInfo { get; private set; }
        public Banco banco { get; private set; }


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

            banco = new Banco();
            var services = new ServiceCollection()
                .AddSingleton(this.banco)
                .AddSingleton(this.ConfigFile)
                .AddSingleton(this.BotInfo)
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
            var prefix = await banco.GetServerPrefixAsync(gld.Id, ConfigFile.PrefixTeste);
#else
            var prefix = await banco.GetServerPrefix(gld.Id, ConfigFile.Prefix);
#endif
            var pfixLocation = msg.GetStringPrefixLength(prefix);
            return await Task.FromResult(pfixLocation);
        }
    }
}
