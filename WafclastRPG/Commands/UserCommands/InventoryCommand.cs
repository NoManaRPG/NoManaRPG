using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using DSharpPlus.Entities;
using MongoDB.Driver;
using DSharpPlus;
using System.Diagnostics;
using DSharpPlus.Interactivity.Extensions;
using System;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using System.Collections.Generic;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class InventoryCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("inventario")]
        [Description("Veja e use itens do seu inventário")]
        [Usage("inventario")]
        [Aliases("inv", "inventory")]
        public async Task InventoryCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    database.StartExecutingInteractivity(ctx.User.Id);

                    var pagina = 1;
                    double maxPag = Convert.ToDouble(await database.CollectionItems.CountDocumentsAsync(session.Session, x => x.PlayerId == ctx.User.Id));
                    maxPag /= 5;

                    DiscordMessage msgEmbed = null;
                    var temporaryInventory = await CreatePlayerInventory(pagina, maxPag, player, msgEmbed, ctx);
                    msgEmbed = temporaryInventory.message;

                    var vity = ctx.Client.GetInteractivity();
                    bool exitLoop = false;
                    while (!exitLoop)
                    {
                        var msg = await vity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.ChannelId == ctx.Channel.Id, TimeSpan.FromMinutes(3));
                        if (msg.TimedOut)
                        {
                            await ctx.ResponderAsync("tempo expirado!");
                            break;
                        }

                        if (int.TryParse(msg.Result.Content, out int id))
                        {
                            id = Math.Clamp(id - 1, 0, 4);
                            var item = temporaryInventory.itens[id];

                            var embed = new DiscordEmbedBuilder();
                            embed.WithDescription(Formatter.BlockCode(item.Description));

                            embed.WithTitle($"{item.Name.Titulo()}");
                            embed.WithThumbnail(item.ImageURL);
                            embed.WithColor(DiscordColor.Blue);
                            embed.AddField("Quantidade".Titulo(), Formatter.InlineCode(item.Quantity.ToString()), true);
                            embed.AddField("Pode empilhar".Titulo(), item.CanStack ? "Sim" : "Não", true);
                            embed.AddField("Pode vender".Titulo(), item.CanSell ? "Sim" : "Não", true);
                            embed.AddField("Preço de compra".Titulo(), $"{Emojis.Coins} {Formatter.InlineCode(item.PriceBuy.ToString())}", true);
                            embed.AddField("Preço de venda".Titulo(), $"{Emojis.Coins} {Formatter.InlineCode((item.PriceBuy / 2).ToString())}", true);
                            embed.AddField("Item ID".Titulo(), Formatter.InlineCode(item.Id.ToString()), true);
                            embed.WithFooter(iconUrl: ctx.User.AvatarUrl);
                            embed.WithTimestamp(DateTime.Now);

                            switch (item)
                            {
                                case WafclastFoodItem wf:
                                    embed.AddField("Cura".Titulo(), wf.LifeGain.ToString("N2"), true);
                                    break;
                            }

                            await ctx.ResponderAsync(embed.Build());
                            break;
                        }

                        switch (msg.Result.Content.ToLower())
                        {
                            case "proximo":
                                if (pagina == maxPag)
                                {
                                    await msg.Result.DeleteAsync();
                                    break;
                                }

                                pagina++;
                                var temp = await CreatePlayerInventory(pagina, maxPag, player, msgEmbed, ctx);
                                msgEmbed = temp.message;
                                await msg.Result.DeleteAsync();
                                break;
                            case "voltar":
                                if (pagina == 1)
                                {
                                    await msg.Result.DeleteAsync();
                                    break;
                                }

                                pagina--;
                                var temp2 = await CreatePlayerInventory(pagina, maxPag, player, msgEmbed, ctx);
                                msgEmbed = temp2.message;
                                await msg.Result.DeleteAsync();
                                break;
                            case "sair":
                                exitLoop = true;
                                break;
                            default:
                                var asd = msgEmbed;
                                msgEmbed = await ctx.RespondAsync(ctx.User.Mention, msgEmbed.Embeds[0]);
                                await msg.Result.DeleteAsync();
                                break;
                        }
                    }
                    database.StopExecutingInteractivity(ctx.User.Id);
                    return new Response("");
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                await ctx.ResponderAsync(response.Message);
                return;
            }
        }

        public async Task<TemporaryInventory> CreatePlayerInventory(int pagina, double maxPag, WafclastPlayer player, DiscordMessage msgEmbed, CommandContext ctx)
        {
            var timer = new Stopwatch();
            timer.Start();

            var itens = await database.CollectionItems.Find(x => x.PlayerId == player.Id)
               .SortByDescending(x => x.Quantity)
               .Skip((pagina - 1) * 5)
               .Limit(5)
               .ToListAsync();

            var embed = new DiscordEmbedBuilder();
            var i = 1;
            foreach (var item in itens)
            {
                if (!item.CanStack)
                    embed.AddField($"{Emojis.GerarNumber(i)}{item.Name}", $"{Formatter.InlineCode("ID:")} {Formatter.InlineCode(item.Id.ToString())}" +
                        $"\n{Formatter.InlineCode("Tipo:")}");
                else
                    embed.AddField($"{Emojis.GerarNumber(i)}{item.Name}", $"{Formatter.InlineCode("Quantidade:")} {Formatter.InlineCode(item.Quantity.ToString())}" +
                        $"\n{Formatter.InlineCode("Tipo:")}");
                i++;
            }

            if (pagina < maxPag)
                embed.AddField($"{Emojis.Direita} Proxima página", $"Escreva {Formatter.InlineCode("proximo")} para ir a proxima página.");

            if (pagina > 1)
                embed.AddField($"{Emojis.Esquerda} Página anterior", $"Escreva {Formatter.InlineCode("voltar")} para ir a página anterior.");

            timer.Stop();

            embed.WithDescription($"{Emojis.Coins} {player.Character.Coins.ToString()}");
            embed.WithFooter($"Digite 1 - 5 para escolher ou sair para fechar | Demorou: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.", ctx.User.AvatarUrl);
            if (msgEmbed == null)
                msgEmbed = await ctx.RespondAsync(ctx.User.Mention, embed.Build());
            else
                msgEmbed = await msgEmbed.ModifyAsync(ctx.User.Mention, embed.Build());
            return new TemporaryInventory(msgEmbed, itens);
        }

        public class TemporaryInventory
        {
            public DiscordMessage message;
            public List<WafclastBaseItem> itens;

            public TemporaryInventory(DiscordMessage message, List<WafclastBaseItem> itens)
            {
                this.message = message;
                this.itens = itens;
            }
        }
    }
}
