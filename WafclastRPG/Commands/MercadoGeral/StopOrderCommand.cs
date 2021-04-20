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
    public class StopOrderCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("mgparar")]
        [Description("Permite parar uma das suas ordens do mercado geral.")]
        [Usage("mgparar <id>")]
        public async Task StopOrderCommandAsync(CommandContext ctx, string objectIdString)
        {
            await ctx.TriggerTypingAsync();

            if (!ObjectId.TryParse(objectIdString, out var id))
            {
                await ctx.ResponderAsync("o ID informado é inválido!");
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
                    if (ordem == null || ordem.PlayerId != ctx.User.Id)
                        return new Response("essa ordem não foi encontrada.");

                    var item = await session.FindItemAsync(ordem.ItemNome, ctx.Client.CurrentUser);

                    // ganha coins
                    if (ordem.Tipo == OrdemType.Venda)
                    {
                        player.Character.Coins.Coins += ordem.Gain;
                        if (ordem.Quantidade != 0)
                            await player.AddItemAsync(item, ordem.Quantidade);
                    }
                    else
                    {
                        // ganha item
                        if (ordem.Gain != 0)
                            await player.AddItemAsync(item, ordem.Gain);
                        player.Character.Coins.Coins += ordem.Quantidade * ordem.Preco;
                    }

                    await session.RemoveAsync(ordem);
                    await player.SaveAsync();

                    return new Response("você cancelou a sua ordem.");
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
