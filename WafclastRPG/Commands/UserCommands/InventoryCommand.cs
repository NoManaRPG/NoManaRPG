using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using DSharpPlus.Entities;
using WafclastRPG.Properties;
using System.Text;
using WafclastRPG.Entities.Itens;

namespace WafclastRPG.Commands.UserCommands
{
    public class InventoryCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("inventario")]
        [Description("Permite verificar a sua mochila")]
        [Usage("inventario < pagina >")]
        [Aliases("inv", "inventory")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task InventoryCommandAsync(CommandContext ctx, int pagina = 0)
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

                    var embed = new DiscordEmbedBuilder();
                    embed.WithTitle("Mochila");

                    var pos = 0;
                    var str1 = new StringBuilder();
                    foreach (var item in cha.Inventory)
                    {
                        ShowInventory(item, str1, pos);

                        if (pos == 9)
                            break;
                    }

                    embed.AddField("Posição | Quantidade | Item", str1.ToString(), true);

                    if (cha.Inventory.Count > 9)
                    {
                        var str2 = new StringBuilder();
                        foreach (var item in cha.Inventory)
                            ShowInventory(item, str1, pos);

                        embed.AddField("Posição | Quantidade | Item", str2.ToString(), true);
                    }

                    embed.WithColor(DiscordColor.Brown);

                    return new Response(embed);
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                await ctx.ResponderAsync(response.Message);
                return;
            }

            await ctx.ResponderAsync(response.Embed.Build());
        }

        public void ShowInventory(WafclastBaseItem item, StringBuilder str, int pos)
        {
            if (item.CanStack)
                str.AppendLine($"[{pos}] - `{item.Quantity}` x **{item.Name}**");
            else
                str.AppendLine($"[{pos}] - **{item.Name}**");
            pos++;
        }
    }

    //    await ctx.TriggerTypingAsync();

    //    Response response;
    //    using (var session = await database.StartDatabaseSessionAsync())
    //        response = await session.WithTransactionAsync(async (s, ct) =>
    //        {
    //            var player = await session.FindPlayerAsync(ctx.User);
    //            if (player == null)
    //                return new Response(Messages.NaoEscreveuComecar);

    //            var embed = new DiscordEmbedBuilder();
    //            var str = new StringBuilder();
    //            str.AppendLine($"{Emojis.Coins} {player.Character.Coins}");
    //            str.AppendLine(Formatter.BlockCode("Quantidade | Item"));

    //            var inventory = await database.CollectionItems.Find(x => x.PlayerId == player.Id)
    //               .SortByDescending(x => x.Quantity)
    //               .Skip((pagina - 1) * 10)
    //               .Limit(10)
    //               .ToListAsync();

    //            foreach (var item in inventory)
    //                str.AppendLine($"`{item.Quantity}` x **{item.Name}**");

    //            embed.WithDescription(str.ToString());
    //            embed.WithFooter($"Pagina {pagina}", ctx.User.AvatarUrl);
    //            embed.WithColor(DiscordColor.Brown);

    //            return new Response(embed);
    //        });

    //    if (!string.IsNullOrWhiteSpace(response.Message))
    //    {
    //        await ctx.ResponderAsync(response.Message);
    //        return;
    //    }

    //    await ctx.ResponderAsync(response.Embed.Build());
}
