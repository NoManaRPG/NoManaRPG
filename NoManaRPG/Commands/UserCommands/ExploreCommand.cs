// This file is part of NoManaRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NoManaRPG.Attributes;
using NoManaRPG.Database;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Database.Response;
using NoManaRPG.Extensions;

namespace NoManaRPG.Commands.UserCommands;

[ModuleLifespan(ModuleLifespan.Transient)]
public class ExploreCommand : BaseCommandModule
{

    private IResponse _res;
    private readonly PlayerRepository _playerRepository;
    private readonly ZoneRepository _zoneRepository;
    private readonly MongoSession _session;
    private readonly UsersBlocked _usersBlocked;

    public ExploreCommand(PlayerRepository playerRepository, ZoneRepository zoneRepository, MongoSession session, UsersBlocked usersBlocked)
    {
        this._playerRepository = playerRepository;
        this._zoneRepository = zoneRepository;
        this._session = session;
        this._usersBlocked = usersBlocked;
    }

    [Command("explore")]
    [Aliases("ex")]
    [Description("Permite explorar uma zona.")]
    [Usage("explorar [nivel]")]
    public async Task StartCommandAsync(CommandContext ctx, int level)
    {
        using (await this._session.StartSessionAsync())
            this._res = await this._session.WithTransactionAsync(async (s, ct) =>
            {
                var zoneWantExplore = await this._zoneRepository.FindPlayerZoneAsync(ctx.Client.CurrentUser.Id, level);
                if (zoneWantExplore == null)
                    return new StringResponse("Nivel não criado ainda!");
                var highestZonePlayer = await this._zoneRepository.FindPlayerHighestZoneAsync(ctx.User.Id);

                if (highestZonePlayer.Level + 1 >= zoneWantExplore.Level)
                    return new StringResponse("Você não explorou a zona anterior!");

                var player = await this._playerRepository.FindPlayerAsync(ctx);
                return new StringResponse("");
            });
    }
}
