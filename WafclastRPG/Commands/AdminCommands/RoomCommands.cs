using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Entities.Wafclast;
using System;
using WafclastRPG.Properties;
using System.Text;
using WafclastRPG.Enums;
using WafclastRPG.Attributes;

namespace WafclastRPG.Commands.AdminCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  [Group("room")]
  [Description("Comandos administrativos de rooms.")]
  [Hidden]
  [RequireOwner]
  public class RoomCommands : BaseCommandModule {
    public DataBase Data { private get; set; }
    public Response Res { private get; set; }
    public TimeSpan timeout = TimeSpan.FromMinutes(2);

    [Command("new")]
    [Description("Permite criar um quarto no canal de texto atual.")]
    public async Task NewRoomCommandAsync(CommandContext ctx) {
      using (var session = await Data.StartDatabaseSessionAsync())
        Res = await session.WithTransactionAsync(async (s, ct) => {

          var name = await ctx.WaitForStringAsync("Informe o Nome do Quarto.", Data, timeout);
          var regionName = await ctx.WaitForStringAsync("Informe o Nome da Região.", Data, timeout);
          var description = await ctx.WaitForStringAsync("Informe uma descrição do quarto.", Data, timeout);
          var vectorX = await ctx.WaitForDoubleAsync("Informe a posição X da localização.", Data, timeout);
          var vectorY = await ctx.WaitForDoubleAsync("Informe a posição Y da localização.", Data, timeout);

          var invite = await ctx.Channel.CreateInviteAsync(0, 0, false, false, "New Room Created");

          var room = new Room() {
            Id = ctx.Channel.Id,
            Name = name.Value,
            Region = regionName.Value,
            Description = description.Value,
            Invite = invite.ToString(),
            Location = new Vector() { X = vectorX.Value, Y = vectorY.Value }
          };

          await session.ReplaceAsync(room);

          var str = new StringBuilder();
          str.AppendLine("**Quarto criado!**");
          str.AppendLine($"Id: `{room.Id}`");
          str.AppendLine($"Nome: {name.Value}");
          str.AppendLine($"Região: {regionName.Value}");
          str.AppendLine($"Descrição: {description.Value}");
          str.AppendLine($"Posição: {vectorX.Value}/{vectorY.Value}");

          return new Response(str.ToString());
        });
      await ctx.ResponderAsync(Res);
    }

    [Command("setdescription")]
    [Description("Permite adicionar uma descrição ao quarto atual.")]
    [Usage("setdescription < description >")]
    public async Task SetRoomDescriptionCommandAsync(CommandContext ctx, [RemainingText] string description) {
      using (var session = await Data.StartDatabaseSessionAsync())
        Res = await session.WithTransactionAsync(async (s, ct) => {
          await ctx.TriggerTypingAsync();

          Room room = null;
          room = await session.FindRoomAsync(ctx.Channel.Id);
          if (room == null)
            return new Response("este lugar não é um quarto.");

          room.Description = description;
          await session.ReplaceAsync(room);

          return new Response($"você alterou a descrição de: **[{room.Name}]!**");
        });
      await ctx.ResponderAsync(Res);
    }
  }
}
