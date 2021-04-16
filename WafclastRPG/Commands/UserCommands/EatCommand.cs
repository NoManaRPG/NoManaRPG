using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class EatCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("comer")]
        [Aliases("eat")]
        [Description("Permite comer um item do tipo Comida")]
        [Usage("comer [ quantidade ] [ nome ]")]
        public async Task UseCommandAsync(CommandContext ctx, int quantity = 1, [RemainingText] string nameItem = "")
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    //Procura jogador
                    var player = await session.FindAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    //Pega item
                    var item = await player.GetItemAsync(nameItem);
                    if (item == null)
                        return new Response($"não foi encontrado o item chamado {Formatter.Bold(nameItem.Titulo())}!");

                    quantity = Math.Abs(quantity);

                    //Usa item
                    switch (item)
                    {
                        case WafclastCookedFoodItem wf:
                            player.Character.Life.Add(wf.LifeGain * quantity);
                            item.Quantity -= quantity;
                            break;
                        default:
                            return new Response($"você não pode comer {nameItem.Titulo()}!");
                    }

                    if (item.Quantity == 0)
                        await session.RemoveAsync(item);
                    else
                        await session.ReplaceAsync(item);
                    await session.ReplaceAsync(player);

                    return new Response($"você comeu {Formatter.Bold($"{quantity} {nameItem.Titulo()}")}!");
                });

            await ctx.ResponderAsync(response.Message);
        }
    }
}
