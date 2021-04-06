using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Enums;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class ShopCommand : BaseCommandModule
    {
        public Database database;

        [Command("loja")]
        [Description("Permite comprar e ver os itens a venda da loja.")]
        [Usage("loja")]
        public async Task ShopCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await database.FindPlayerAsync(ctx);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var map = await database.FindMapAsync(player.Character.Localization.ChannelId);
            if (map.Tipo != MapType.Cidade)
            {
                await ctx.ResponderAsync(Strings.SomenteNaCidade);
                return;
            }

            if (map.ShopItens.Count == 0)
            {
                await ctx.ResponderAsync("Está loja não tem itens a venda!");
                return;
            }


            var embed = new DiscordEmbedBuilder();
            embed.WithTitle("Loja");

            var str = new StringBuilder();
            int ind = 1;
            foreach (var itens in map.ShopItens)
            {
                var coins = new WafclastCoins((ulong)itens.Price);
                str.AppendLine($"{Emojis.GerarNumber(ind)} {itens.Name} - {Emojis.Coins} {coins} para comprar");
                ind++;
            }



            database.StartExecutingInteractivity(ctx);

            var itemDesejado = await ctx.WaitForIntAsync(str.ToString(), database, minValue: 1, maxValue: map.ShopItens.Count);
            if (itemDesejado.TimedOut)
                return;
            var item = map.ShopItens[itemDesejado.Result - 1];

            var quantidadeDesejada = await ctx.WaitForIntAsync("Quantos itens você deseja comprar?", database, minValue: 1);
            if (quantidadeDesejada.TimedOut)
                return;

            if (quantidadeDesejada.Result * item.Price > Convert.ToInt32(player.Character.Coins.Coins))
            {
                await ctx.ResponderAsync("você não tem moedas o suficiente!");
                return;
            }

            using (var session = await this.database.StartDatabaseSessionAsync())
            {
                var result = await session.WithTransactionAsync(async (s, ct) =>
                 {
                     var ff = await session.FindPlayerAsync(ctx.User);
                     await ff.ItemAdd(item, quantidadeDesejada.Result);
                     ff.Character.Coins.Coins -= Convert.ToUInt64(quantidadeDesejada.Result * item.Price);
                     await ff.SaveAsync();
                     return Task.CompletedTask;
                 });
            }

            await ctx.ResponderAsync($"você comprou {quantidadeDesejada.Result} {item.Name}!");
        }
    }
}
