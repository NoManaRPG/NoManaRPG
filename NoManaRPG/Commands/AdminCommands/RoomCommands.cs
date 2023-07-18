// This file is part of NoManaRPG project.

using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NoManaRPG.Attributes;
using NoManaRPG;
using NoManaRPG.Repositories;
using NoManaRPG.Extensions;

namespace NoManaRPG.Commands.AdminCommands;

[ModuleLifespan(ModuleLifespan.Transient)]
[Group("room")]
[Description("Comandos administrativos de rooms.")]
[Hidden]
[RequireOwner]
public class RoomCommands : BaseCommandModule
{
    private readonly PlayerRepository _playerRepository;
    private readonly ZoneRepository _zoneRepository;
    private readonly UsersBlocked _usersBlocked;
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(2);

    public RoomCommands(PlayerRepository playerRepository, ZoneRepository zoneRepository, UsersBlocked usersBlocked)
    {
        this._playerRepository = playerRepository;
        this._zoneRepository = zoneRepository;
        this._usersBlocked = usersBlocked;
    }

    public TimeSpan timeout = TimeSpan.FromMinutes(2);

    [Command("newlevel")]
    [Description("Permite criar um nível novo.")]
    public async Task NewLevelAsync(CommandContext ctx)
    {


        var str = new StringBuilder();
        str.AppendLine("**Quarto criado!**");

        await CommandContextExtension.RespondAsync(ctx, str.ToString());
    }

    [Command("novo-monstro")]
    [Description("Permite criar um monstro no canal de texto atual.")]
    public async Task NewMonsterCommandAsync(CommandContext ctx, ulong channel, [RemainingText] string name)
    {

        //var room = await this._zoneRepository.FindZoneOrDefaultAsync(channel);
        //if (room == null)
        //{
        //    await CommandContextExtension.RespondAsync(ctx, "este lugar não é um quarto.");
        //    return;
        //}


        //await this._zoneRepository.SaveZoneAsync(room);


        //var str = new StringBuilder();
        //str.AppendLine("**Monstro criado!**");

        //await CommandContextExtension.RespondAsync(ctx, str.ToString());
    }

    [Command("setdescription")]
    [Description("Permite adicionar uma descrição ao quarto atual.")]
    [Usage("setdescription < description >")]
    public async Task SetRoomDescriptionCommandAsync(CommandContext ctx, [RemainingText] string description)
    {

        //await ctx.TriggerTypingAsync();

        //WafclastZone room = null;
        //room = await this._zoneRepository.FindZoneOrDefaultAsync(ctx.Channel.Id);
        //if (room == null)
        //    await CommandContextExtension.RespondAsync(ctx, "este lugar não é um quarto.");

        //await this._zoneRepository.SaveZoneAsync(room);

        //await CommandContextExtension.RespondAsync(ctx, $"você alterou a descrição de: **[{room.Name}]!**");
    }
}
