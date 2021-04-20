using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.MercadoGeral;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.MercadoGeral
{
    public class MyOrdersCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("mglista")]
        [Description("Permite ver minhas ordens criadas")]
        [Usage("mglista")]
        public async Task UseCommandAsync(CommandContext ctx, int pagina = 1)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    pagina = Math.Abs(pagina);
                    var embed = new DiscordEmbedBuilder();
                    var orders = await database.CollectionOrdens.Find(x => x.PlayerId == player.Id)
                       .Skip((pagina - 1) * 10)
                       .Limit(10)
                       .ToListAsync();

                    foreach (var ordem in orders)
                        if (ordem.Tipo == OrdemType.Venda)
                            embed.AddField($"`{ordem.Id}` - VENDA", $"`{ordem.Quantidade}` ainda disponível.  {Emojis.Coins} `{ordem.Preco:N0}` cada.");
                        else
                            embed.AddField($"`{ordem.Id}` - COMPRA", $"`{ordem.Quantidade}` ainda disponível.  {Emojis.Coins} `{ordem.Preco:N0}` cada.");

                    embed.WithFooter($"Pagina {pagina}", ctx.User.AvatarUrl);
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
