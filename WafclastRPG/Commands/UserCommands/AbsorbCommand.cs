using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class AbsorbCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("absorver")]
        [Aliases("absorb")]
        [Description("Utilizando metodos antigos, permite você absorver a energia de cristais.")]
        [Usage("absorver <quantidade> <item>")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task AbsorbCommandAsync(CommandContext ctx, ulong quantity, [RemainingText] string itemName)
        {
            await ctx.TriggerTypingAsync();

            //Response response;
            //using (var session = await database.StartDatabaseSessionAsync())
            //    response = await session.WithTransactionAsync(async (s, ct) =>
            //    {
            //        var player = await session.FindPlayerAsync(ctx.User);
            //        if (player == null)
            //            return new Response(Messages.NaoEscreveuComecar);

            //        var item = await player.GetItemAsync(itemName);
            //        if (item == null)
            //            return new Response($"você não tem {itemName} no seu inventário!");

            //        if (item is WafclastMonsterCoreItem is false)
            //            return new Response($"o item não é um Cristal.");

            //        if (quantity > item.Quantity)
            //            return new Response($"você somente tem {item.Quantity} x {item.Name}!");

            //        var cristal = item as WafclastMonsterCoreItem;

            //        player.Character.AddExperience(cristal.ExperienceGain * quantity);
            //        item.Quantity -= quantity;

            //        await player.SaveItemAsync(item);
            //        await session.ReplaceAsync(player);

            //        return new Response($"você absorveu {quantity} x {cristal.Name} e ganhou `{(quantity * cristal.ExperienceGain):N2}` de experiencia!");
            //    });

            //await ctx.ResponderAsync(response.Message);
        }
    }
}
