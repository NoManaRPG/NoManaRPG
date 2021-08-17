using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class EquipCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("equipar")]
        [Aliases("eq")]
        [Description("Permite equipar algum objeto.")]
        [Usage("equipar [ slot do inventário ]")]
        public async Task EquipCommandAsync(CommandContext ctx, int slot = 0)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);
                    var cha = player.Character;

                    if (cha.Inventory.Count == 0)
                        return new Response("a sua mochila está vazia!");
                    slot = Math.Clamp(slot, 0, 19);

                    var item = cha.Inventory[slot];
                    if (item is WafclastEquipableItem)
                    {
                        cha.TryEquipItem(item as WafclastEquipableItem);
                    }
                    else
                        return new Response($"por que você iria tentar equipar: **{item.Name}**?");
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }
    }
}