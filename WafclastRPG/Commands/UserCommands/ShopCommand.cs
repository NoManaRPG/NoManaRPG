using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;

namespace WafclastRPG.Commands.UserCommands
{
    public class ShopCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("loja")]
        [Description("Permite comprar e ver os itens a venda da loja.")]
        [Usage("loja")]
        public async Task ShopCommandAsync(CommandContext ctx)
        {
            //await ctx.TriggerTypingAsync();
            //var player = await database.FindAsync(ctx.User);
            //if (player.Character == null)
            //{
            //    await ctx.ResponderAsync(Strings.NovoJogador);
            //    return;
            //}

            //var map = await database.FindAsync(player.Character.Localization);
            //if (map.Tipo != MapType.Cidade || player.Character.Localization != map)
            //{
            //    await ctx.ResponderAsync(Strings.SomenteNaCidade);
            //    return;
            //}

            //if (map.ShopItens.Count == 0)
            //{
            //    await ctx.ResponderAsync("esta loja não tem itens a venda!");
            //    return;
            //}

            //var embed = new DiscordEmbedBuilder();
            //embed.WithTitle("Loja");

            //var str = new StringBuilder();
            //int ind = 1;
            //foreach (var itens in map.ShopItens)
            //{
            //    var coins = new WafclastCoins((ulong)itens.Price);
            //    str.AppendLine($"{Emojis.GerarNumber(ind)} {itens.Name} - {Emojis.Coins} {coins} para comprar");
            //    ind++;
            //}

            //var itemDesejado = await ctx.WaitForIntAsync(str.ToString(), database, minValue: 1, maxValue: map.ShopItens.Count);
            //if (itemDesejado.TimedOut)
            //    return;
            //var item = map.ShopItens[itemDesejado.Value - 1];

            //var quantidadeDesejada = await ctx.WaitForIntAsync("Quantos itens você deseja comprar?", database, minValue: 1);
            //if (quantidadeDesejada.TimedOut)
            //    return;

            //if (quantidadeDesejada.Value * item.Price > Convert.ToInt32(player.Character.Coins.Coins))
            //{
            //    await ctx.ResponderAsync("você não tem moedas o suficiente!");
            //    return;
            //}

            //await database.InsertAsync(item, quantidadeDesejada.Value, player);
            //player.Character.Coins.Subtract(quantidadeDesejada.Value * item.Price);
            //await database.ReplaceAsync(player);

            //await ctx.ResponderAsync($"você comprou {quantidadeDesejada.Value} {item.Name}!");
        }
    }
}
