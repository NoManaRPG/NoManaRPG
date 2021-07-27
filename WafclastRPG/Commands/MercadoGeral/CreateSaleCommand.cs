using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.MercadoGeral;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.MercadoGeral
{
    class CreateSaleCommand : BaseCommandModule
    {
        public DataBase banco = null;

        [Command("mgcriarvenda")]
        [Description("Permite criar uma ordem de venda no mercado geral.")]
        [Usage("mgcriarvenda <preço> <quantidade> <item>")]
        public async Task UseCommandAsync(CommandContext ctx, ulong preco, ulong quantidade, [RemainingText] string nameItem)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var item = await player.GetItemAsync(nameItem);

                    if (item == null)
                        return new Response("você não tem este item para vender!");

                    if (item.CanSell == false)
                        return new Response("você não pode vender este item!");

                    if (item.Quantity < quantidade)
                        return new Response($"você somente tem {item.Quantity} x {item.Name}!");

                    var ordem = new Ordem
                    {
                        Tipo = OrdemType.Venda,
                        PlayerId = player.Id,
                        ItemNome = item.Name,
                        Quantidade = quantidade,
                        Preco = preco,
                        Ativa = true
                    };

                    item.Quantity -= quantidade;
                    await player.SaveItemAsync(item);
                    await session.InsertAsync(ordem);

                    var embed = new DiscordEmbedBuilder();
                    embed.WithColor(DiscordColor.Brown);
                    embed.WithAuthor($"{ctx.User.Username}", iconUrl: ctx.User.AvatarUrl);
                    embed.WithTitle($"Está vendendo {item.Name}!");
                    embed.WithDescription(Formatter.BlockCode(item.Description));
                    embed.AddField("Preço por cada", $"{Emojis.Coins} {ordem.Preco:N0}", true);
                    embed.AddField("Quantidade a venda", ordem.Quantidade.ToString(), true);
                    embed.AddField("Para comprar digite", $"`w.mgcomprar {ordem.Quantidade} {ordem.Id}`");
                    embed.WithTimestamp(DateTime.Now);
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
