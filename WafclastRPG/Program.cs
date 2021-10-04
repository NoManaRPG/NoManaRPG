// This file is part of the WafclastRPG project.

using System.Configuration;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WafclastRPG.Database;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Database.Repositories;

namespace WafclastRPG
{
    public class Program
    {
        public MongoDbContext MongoDbContext { get; private set; }
        public UsersBlocked UsersTemporaryBlocked { get; private set; }

        static void Main() => new Program().RodarBotAsync().GetAwaiter().GetResult();

        public async Task RodarBotAsync()
        {
            var bot = new Bot(new DiscordConfiguration
            {
                TokenType = TokenType.Bot,
                ReconnectIndefinitely = true,
                GatewayCompressionLevel = GatewayCompressionLevel.Stream,
                AutoReconnect = true,
                Token = ConfigurationManager.AppSettings.Get("Token"),
                Intents = DiscordIntents.AllUnprivileged,
                MinimumLogLevel = LogLevel.Debug,
            });

            this.MongoDbContext = new MongoDbContext(ConfigurationManager.ConnectionStrings["MongoConnection"].ConnectionString);
            this.UsersTemporaryBlocked = new UsersBlocked();
            var services = new ServiceCollection()
                .AddSingleton(this.MongoDbContext)
                .AddSingleton(this.UsersTemporaryBlocked)
                .AddScoped<IMongoSession, MongoSession>()
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<IItemRepository, ItemRepository>()
                .AddScoped<IRoomRepository, RoomRepository>()
                .BuildServiceProvider();

            bot.ModuleCommand(new CommandsNextConfiguration
            {
                PrefixResolver = ResolvePrefixAsync,
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
            // Private Messages
            var gld = msg.Channel.Guild;
            if (gld == null)
                return await Task.FromResult(-1);
            if (this.UsersTemporaryBlocked.IsUserBlocked(msg.Author.Id))
                return await Task.FromResult(-1);

            var prefix = await this.MongoDbContext.GetServerPrefixAsync(gld.Id, ConfigurationManager.AppSettings.Get("Prefix"));
            var pfixLocation = msg.GetStringPrefixLength(prefix);
            return await Task.FromResult(pfixLocation);
        }
    }
}
