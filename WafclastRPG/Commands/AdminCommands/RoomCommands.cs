using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Extensions;
using WafclastRPG.Entities.Wafclast;
using System;
using System.Text;
using WafclastRPG.Attributes;
using WafclastRPG.Entities;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Commands.AdminCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  [Group("room")]
  [Description("Comandos administrativos de rooms.")]
  [Hidden]
  [RequireOwner]
  public class RoomCommands : BaseCommandModule {
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IInteractivityRepository _interactivityRepository;

    public RoomCommands(IPlayerRepository playerRepository, IRoomRepository roomRepository, IInteractivityRepository interactivityRepository) {
      _playerRepository = playerRepository;
      _roomRepository = roomRepository;
      _interactivityRepository = interactivityRepository;
    }
    public TimeSpan timeout = TimeSpan.FromMinutes(2);

    [Command("new")]
    [Description("Permite criar um quarto no canal de texto atual.")]
    public async Task NewRoomCommandAsync(CommandContext ctx) {


      var interac = new Interactivity(_interactivityRepository, ctx);

      var asdname = await interac.WaitForStringAsync("Informe o Nome do Quarto.");

      var name = await interac.WaitForStringAsync("Informe o Nome do Quarto.");
      var regionName = await interac.WaitForStringAsync("Informe o Nome da Região.");
      var description = await interac.WaitForStringAsync("Informe uma descrição do quarto.");
      var vectorX = await interac.WaitForDoubleAsync("Informe a posição X da localização.");
      var vectorY = await interac.WaitForDoubleAsync("Informe a posição Y da localização.");

      var invite = await ctx.Channel.CreateInviteAsync(0, 0, false, false, "New Room Created");

      var room = new Room() {
        Id = ctx.Channel.Id,
        Name = name.Value,
        Region = regionName.Value,
        Description = description.Value,
        Invite = invite.ToString(),
        Location = new Vector() { X = vectorX.Value, Y = vectorY.Value }
      };

      await _roomRepository.SaveRoomAsync(room);

      var str = new StringBuilder();
      str.AppendLine("**Quarto criado!**");
      str.AppendLine($"Id: `{room.Id}`");
      str.AppendLine($"Nome: {name.Value}");
      str.AppendLine($"Região: {regionName.Value}");
      str.AppendLine($"Descrição: {description.Value}");
      str.AppendLine($"Posição: {vectorX.Value}/{vectorY.Value}");

      await ctx.ResponderAsync(str.ToString());
    }

    [Command("setdescription")]
    [Description("Permite adicionar uma descrição ao quarto atual.")]
    [Usage("setdescription < description >")]
    public async Task SetRoomDescriptionCommandAsync(CommandContext ctx, [RemainingText] string description) {

      await ctx.TriggerTypingAsync();

      Room room = null;
      room = await _roomRepository.FindRoomOrDefaultAsync(ctx.Channel.Id);
      if (room == null)
        await ctx.ResponderAsync("este lugar não é um quarto.");

      room.Description = description;
      await _roomRepository.SaveRoomAsync(room);

      await ctx.ResponderAsync($"você alterou a descrição de: **[{room.Name}]!**");
    }

    [Command("setcoordinates")]
    [Description("Permite adicionar uma coordenada ao quarto atual.")]
    [Usage("setcoordinates < X > < Y >")]
    public async Task SetRoomCoordinatesCommandAsync(CommandContext ctx, double x, double y) {

      await ctx.TriggerTypingAsync();

      Room room = null;
      room = await _roomRepository.FindRoomOrDefaultAsync(ctx.Channel.Id);
      if (room == null)
        await ctx.ResponderAsync("este lugar não é um quarto.");

      room.Location = new Vector(x, y);
      await _roomRepository.SaveRoomAsync(room);


      await ctx.ResponderAsync($"você alterou as coordenadas de: **[{room.Name}]!**");
    }
  }
}
