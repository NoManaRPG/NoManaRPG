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

                    var itemType = cha.Inventory[slot];

                    switch (itemType)
                    {
                        case WafclastWeaponItem wwi:
                            switch (wwi.Slot)
                            {
                                case SlotEquipament.MainHand:
                                    cha.MainHand = wwi;
                                    await EquipAndSave(player, wwi);
                                    return new Response($"você equipou: **{itemType.Name}** na mão principal.");
                                case SlotEquipament.OffHand:
                                    cha.OffHand = wwi;
                                    await EquipAndSave(player, wwi);
                                    return new Response($"você equipou: **{itemType.Name}** na mão secundaria.");
                            }
                            break;
                    }

                    return new Response($"por que você iria tentar equipar: **{itemType.Name}**?");
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }

        public async Task EquipAndSave(WafclastPlayer player, WafclastWeaponItem wwi)
        {
            player.Character.Inventory.Remove(wwi);
            player.Character.Accuracy = player.Character.CalculateAccuracy(wwi.Accuracy);
            await player.SaveAsync();
        }
    }
}