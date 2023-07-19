// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Extensions;
using NoManaRPG.Interactivity;

namespace NoManaRPG.Comandos.UserCommands;

[ModuleLifespan(ModuleLifespan.Transient)]
public class ExploreComando : BaseCommandModule
{

    private readonly PlayerRepository _playerRepository;
    private readonly ZoneRepository _zoneRepository;
    private readonly MongoSession _session;
    private readonly UsersBlocked _usersBlocked;

    public ExploreComando(PlayerRepository playerRepository, ZoneRepository zoneRepository, MongoSession session, UsersBlocked usersBlocked)
    {
        this._playerRepository = playerRepository;
        this._zoneRepository = zoneRepository;
        this._session = session;
        this._usersBlocked = usersBlocked;
    }

    [Command("explore")]
    [Aliases("ex")]
    [Description("Permite explorar uma zona.")]
    public async Task StartCommandAsync(CommandContext ctx, int level)
    {
        using (await this._session.StartSessionAsync())
            await this._session.Session.WithTransactionAsync(async (s, ct) =>
            {
                //var zoneWantExplore = await this._zoneRepository.FindPlayerZoneAsync(ctx.Client.CurrentUser.Id, level);
                //if (zoneWantExplore == null)
                //    return new StringResponse("Nivel não criado ainda!");
                var highestZonePlayer = await this._zoneRepository.FindPlayerHighestZoneAsync(ctx.User.Id);

                //if (highestZonePlayer.Level + 1 >= zoneWantExplore.Level)
                //    return new StringResponse("Você não explorou a zona anterior!");

                //var player = await this._playerRepository.FindPlayerAsync(ctx);
                return Task.CompletedTask;
            });
    }
}
