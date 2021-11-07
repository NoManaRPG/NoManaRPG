// This file is part of the WafclastRPG project.

using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using WafclastRPG.Attributes;
using WafclastRPG.Database;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Commands.AdminCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Group("room")]
    [Description("Comandos administrativos de rooms.")]
    [Hidden]
    [RequireOwner]
    public class RoomCommands : BaseCommandModule
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly UsersBlocked _usersBlocked;
        private readonly TimeSpan _timeout = TimeSpan.FromMinutes(2);

        public RoomCommands(IPlayerRepository playerRepository, IZoneRepository zoneRepository, UsersBlocked usersBlocked)
        {
            this._playerRepository = playerRepository;
            this._zoneRepository = zoneRepository;
            this._usersBlocked = usersBlocked;
        }

        public TimeSpan timeout = TimeSpan.FromMinutes(2);

        [Command("new")]
        [Description("Permite criar um quarto no canal de texto atual.")]
        public async Task NewRoomCommandAsync(CommandContext ctx)
        {

            var interac = new Interactivity(this._usersBlocked, ctx);

            var asdname = await interac.WaitForStringAsync("Informe o Nome do Quarto.");

            var name = await interac.WaitForStringAsync("Informe o Nome do Quarto.");
            var regionName = await interac.WaitForStringAsync("Informe o Nome da Região.");
            var description = await interac.WaitForStringAsync("Informe uma descrição do quarto.");
            var vectorX = await interac.WaitForDoubleAsync("Informe a posição X da localização.");
            var vectorY = await interac.WaitForDoubleAsync("Informe a posição Y da localização.");

            var invite = await ctx.Channel.CreateInviteAsync(0, 0, false, false, "New Room Created");

            var room = new WafclastZone()
            {
                PlayerId = ctx.Channel.Id,
                Name = name.Result,
            };

            await this._zoneRepository.SaveZoneAsync(room);

            var str = new StringBuilder();
            str.AppendLine("**Quarto criado!**");
            str.AppendLine($"Id: `{room.PlayerId}`");
            str.AppendLine($"Nome: {name.Result}");
            str.AppendLine($"Região: {regionName.Result}");
            str.AppendLine($"Descrição: {description.Result}");
            str.AppendLine($"Posição: {vectorX.Result}/{vectorY.Result}");

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

        [Command("setcoordinates")]
        [Description("Permite adicionar uma coordenada ao quarto atual.")]
        [Usage("setcoordinates < X > < Y >")]
        public async Task SetRoomCoordinatesCommandAsync(CommandContext ctx, double x, double y)
        {

            //await ctx.TriggerTypingAsync();

            //WafclastZone room = null;
            //room = await this._zoneRepository.FindZoneOrDefaultAsync(ctx.Channel.Id);
            //if (room == null)
            //    await CommandContextExtension.RespondAsync(ctx, "este lugar não é um quarto.");

            //await this._zoneRepository.SaveZoneAsync(room);


            //await CommandContextExtension.RespondAsync(ctx, $"você alterou as coordenadas de: **[{room.Name}]!**");
        }
    }
}
