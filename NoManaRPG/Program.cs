// This file is part of NoManaRPG project.

using System.Configuration;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NoManaRPG.Comandos;
using NoManaRPG.Comandos.AdminCommands;
using NoManaRPG.Comandos.UserComandos;
using NoManaRPG.Comandos.UserCommands;
using NoManaRPG.Database;
using NoManaRPG.Database.Repositories;
using NoManaRPG.DiscordEvents;

namespace NoManaRPG;

public class Program
{
    public static async Task Main(string[] args)
    {
        #region Configs
#if DEBUG
        var map = new ExeConfigurationFileMap { ExeConfigFilename = "App.Debug.config" };
#else
        var map = new ExeConfigurationFileMap { ExeConfigFilename = "App.Release.config" };
#endif
        var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        #endregion

        var mongoDbContext = new DbContext(config.ConnectionStrings.ConnectionStrings["MongoConnection"].ConnectionString);
        //var usersTemporaryBlocked = new UsersBlocked();

        var client = new DiscordClient(new DiscordConfiguration
        {
            ReconnectIndefinitely = true,
            AutoReconnect = true,
            Token = config.AppSettings.Settings["Token"].Value,
            Intents = DiscordIntents.AllUnprivileged,
            MinimumLogLevel = LogLevel.Debug,
        });

        var services = new ServiceCollection()
            .AddSingleton(mongoDbContext)
            //.AddSingleton(usersTemporaryBlocked)
            .AddSingleton(config)
            .AddScoped<MongoSession>()
            .AddScoped<PlayerRepository>()
            .AddScoped<ItemRepository>()
            .AddScoped<ZoneRepository>()
            .BuildServiceProvider();

        var slashCommands = client.UseSlashCommands(new SlashCommandsConfiguration()
        {
            Services = services,
        });

        const ulong servidor = 1118002801046474773;

        slashCommands.RegisterCommands<AdminComando>(servidor);
        slashCommands.RegisterCommands<StatusComando>();
        slashCommands.RegisterCommands<CriarPersonagemComando>();
        slashCommands.RegisterCommands<InfoComando>();

        slashCommands.SlashCommandErrored += SlashCommandErrorEvent.EventAsync;

        client.Ready += (c, e) => ReadyEvent.Event(c, e, config);
        client.GuildAvailable += (c, e) => GuildAvailableEvent.Event(c, e);
        client.ClientErrored += ClientErroredEvent.Event;

        await client.ConnectAsync();
        await Task.Delay(-1);
    }


    //private async Task<int> ResolvePrefixAsync(DiscordMessage msg)
    //{
    //    // Private Messages
    //    var gld = msg.Channel.Guild;
    //    if (gld == null)
    //        return await Task.FromResult(-1);
    //    if (UsersTemporaryBlocked.IsUserBlocked(msg.Author.Id))
    //        return await Task.FromResult(-1);

    //    var prefix = await MongoDbContext.GetServerPrefixAsync(gld.Id, Config.AppSettings.Settings["Prefix"].Value);
    //    var pfixLocation = msg.GetStringPrefixLength(prefix);
    //    return await Task.FromResult(pfixLocation);
    //}
}

