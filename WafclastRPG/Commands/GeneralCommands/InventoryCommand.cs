using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using DSharpPlus.Entities;
using System.Text;
using MongoDB.Driver;
using DSharpPlus;
using System.Diagnostics;
using DSharpPlus.Interactivity.Extensions;
using System;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using System.Collections.Generic;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class InventoryCommand : BaseCommandModule
    {
        public Database banco;

        [Command("inventario")]
        [Description("Veja e use itens do seu inventário")]
        [Usage("inventario")]
        [Aliases("inv", "inventory")]
        public async Task InventoryCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx.User);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            banco.StartExecutingInteractivity(ctx.User.Id);

            var pagina = 1;
            var maxPag = (int)Math.Ceiling((double)player.Character.Inventory.QuantityDifferentItens / 5);

            var msgEmbed = await ctx.ResponderAsync("carregando inventário...");
            var temporaryInventory = await CreatePlayerInventory(pagina, maxPag, player, msgEmbed, ctx);

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
                    id = Math.Clamp(id - 1, 0, 5);
                    var item = temporaryInventory.itens[id];

                    var embed = new DiscordEmbedBuilder();
                    embed.WithDescription(Formatter.BlockCode(item.Description));

                    embed.WithTitle($"{item.Name.Titulo()}");
                    embed.WithThumbnail(item.ImageURL);
                    embed.WithColor(DiscordColor.Blue);
                    embed.AddField("Quantidade".Titulo(), Formatter.InlineCode(item.Quantity.ToString()), true);
                    embed.AddField("Preço de compra".Titulo(), $"{Emojis.Coins} {Formatter.InlineCode(item.Price.ToString())}", true);
                    embed.AddField("Preço de venda".Titulo(), $"{Emojis.Coins} {Formatter.InlineCode((item.Price / 2).ToString())}", true);
                    embed.AddField("Tipo".Titulo(), item.Type.GetEnumDescription(), true);
                    embed.AddField("Nível".Titulo(), item.Level.ToString(), true);
                    embed.AddField("Pode vender".Titulo(), item.CanSell ? "Sim" : "Não", true);
                    embed.AddField("Pode empilhar".Titulo(), item.CanStack ? "Sim" : "Não", true);
                    embed.WithFooter($"ID: {item.ItemID}",ctx.User.AvatarUrl);
                    embed.WithTimestamp(DateTime.Now);
                    await ctx.ResponderAsync(embed.Build());
                    break;
                }

                switch (msg.Result.Content)
                {
                    case "proximo":
                        pagina++;
                        await CreatePlayerInventory(pagina, maxPag, player, msgEmbed, ctx);
                        await msg.Result.DeleteAsync();
                        break;
                    case "voltar":
                        if (pagina == 1)
                        {
                            await msg.Result.DeleteAsync();
                            break;
                        }

                        pagina--;
                        await CreatePlayerInventory(pagina, maxPag, player, msgEmbed, ctx);
                        await msg.Result.DeleteAsync();
                        break;
                    case "sair":
                        exitLoop = true;
                        break;
                    default:
                        msgEmbed = await ctx.RespondAsync(ctx.User.Mention, msgEmbed.Embeds[0]);
                        await msg.Result.DeleteAsync();
                        break;
                }
            }
            banco.StopExecutingInteractivity(ctx.User.Id);
        }

        public async Task<TemporaryInventory> CreatePlayerInventory(int pagina, int maxPag, WafclastPlayer player, DiscordMessage msgEmbed, CommandContext ctx)
        {
            var timer = new Stopwatch();
            timer.Start();

            var itens = await banco.CollectionItens.Find(x => x.PlayerId == player.Id)
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
                        $"\n{Formatter.InlineCode("Tipo:")} {Formatter.InlineCode(item.Type.GetEnumDescription())}");
                else
                    embed.AddField($"{Emojis.GerarNumber(i)}{item.Name}", $"{Formatter.InlineCode("Quantidade:")} {Formatter.InlineCode(item.Quantity.ToString())}" +
                        $"\n{Formatter.InlineCode("Tipo:")} {Formatter.InlineCode(item.Type.GetEnumDescription())}");
                i++;
            }

            if (pagina < maxPag)
                embed.AddField($"{Emojis.Direita} Proxima página", $"Escreva {Formatter.InlineCode("proximo")} para ir a proxima página.");

            if (pagina > 1)
                embed.AddField($"{Emojis.Esquerda} Página anterior", $"Escreva {Formatter.InlineCode("voltar")} para ir a página anterior.");

            timer.Stop();

            embed.WithDescription($"{Emojis.Coins} {player.Character.Coins.ToString()}");
            embed.WithFooter($"Digite um número para escolher ou {Formatter.InlineCode("sair")} para fechar | Demorou: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.", ctx.User.AvatarUrl);
            return new TemporaryInventory(await msgEmbed.ModifyAsync(ctx.User.Mention, embed.Build()), itens);
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
