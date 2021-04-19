using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using DSharpPlus.Entities;
using MongoDB.Driver;
using DSharpPlus;
using System;
using WafclastRPG.Properties;
using System.Text;

namespace WafclastRPG.Commands.UserCommands
{
    public class InventoryCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("inventario")]
        [Description("Permite ver os itens do seu inventário")]
        [Usage("inventario <pagina>")]
        [Aliases("inv", "inventory")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task InventoryCommandAsync(CommandContext ctx, int pagina = 1)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var embed = new DiscordEmbedBuilder();
                    var str = new StringBuilder();
                    str.AppendLine($"{Emojis.Coins} {player.Character.Coins}");
                    str.AppendLine(Formatter.BlockCode("Quantidade | Item"));

                    double maxPag = Convert.ToDouble(await database.CollectionItems.CountDocumentsAsync(session.Session, x => x.PlayerId == ctx.User.Id));
                    maxPag /= 10;

                    var inventory = await database.CollectionItems.Find(x => x.PlayerId == player.Id)
                       .SortByDescending(x => x.Quantity)
                       .Skip((pagina - 1) * 10)
                       .Limit(10)
                       .ToListAsync();


                    foreach (var item in inventory)
                        str.AppendLine($"`{item.Quantity}` x **{item.Name}**");

                    embed.WithDescription(str.ToString());
                    embed.WithFooter($"Pagina {pagina}/{Math.Ceiling(maxPag):N0}", ctx.User.AvatarUrl);
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
    }
}
