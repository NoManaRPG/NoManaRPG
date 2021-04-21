using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Bson;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.MercadoGeral;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.MercadoGeral
{
    class BuyCommand : BaseCommandModule
    {
        public DataBase banco = null;

        [Command("mgcomprar")]
        [Description("Permite comprar de uma ordem de venda no mercado geral.")]
        [Usage("mgcomprar <quantidade> <id>")]
        public async Task UseCommandAsync(CommandContext ctx, ulong quantidade, string objectIdString)
        {
            await ctx.TriggerTypingAsync();

            if (!ObjectId.TryParse(objectIdString, out var id))
            {
                await ctx.ResponderAsync("você precisa informar um ID válido.");
                return;
            }

            if (quantidade == 0)
            {
                await ctx.ResponderAsync("você precisa informar no minimo 1 de quantidade.");
                return;
            }

            Response response;
            using (var session = await banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var ordem = await session.FindOrdemAsync(id);
                    if (ordem == null || ordem.Ativa == false)
                        return new Response("essa ordem não está mais ativa.");

                    if (ordem.Tipo == OrdemType.Compra)
                        return new Response("essa é uma ordem de compra, e não de venda.");

                    if (quantidade > ordem.Quantidade)
                        return new Response("você informou uma quantidade não disponível para essa ordem de venda.");

                    var preco = ordem.Preco * quantidade;

                    if (preco > player.Character.Coins.Coins)
                        return new Response("você não tem moedas o suficiente.");

                    ordem.Quantidade -= quantidade;
                    player.Character.Coins.Coins -= preco;
                    ordem.Gain += preco;

                    if (ordem.Quantidade == 0)
                        ordem.Ativa = false;
                    await session.ReplaceAsync(ordem);

                    var item = await session.FindItemAsync(ordem.ItemNome, ctx.Client.CurrentUser);
                    await player.AddItemAsync(item, quantidade);
                    await player.SaveAsync();

                    //var acao = await session.FindAcaoAsync(ordem.ItemNome);
                    //acao.QuantidadeNegociado += quantidade;
                    //acao.ValoresNegociados += preco;
                    //await session.ReplaceAsync(acao);

                    return new Response($"você comprou {quantidade} x {item.Name} de `{ordem.Id}`.");
                });

            await ctx.ResponderAsync(response.Message);
            return;
        }
    }
}
