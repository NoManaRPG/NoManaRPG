using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class EatCommand : BaseCommandModule
    {
        public Database banco;

        //Separar entre USAR, COMER, BEBER, EQUIPAR
        [Command("comer")]
        [Aliases("eat")]
        [Description("Permite comer um item do tipo Comida")]
        [Usage("comer [ quantidade ] [ nome ]")]
        [Example("comer 2 frango ", "Come duas vezes o item chamado frango")]
        public async Task UseCommandAsync(CommandContext ctx, int quantity = 1, [RemainingText] string itemName = "")
        {
            await ctx.TriggerTypingAsync();

            Task<Response> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    //Procura jogador
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return Task.FromResult(new Response() { IsPlayerFound = false });

                    //Pega item
                    var item = await session.FindItemByNameAsync(itemName, player.Id);
                    if (item == null)
                        return Task.FromResult(new Response() { IsItemFound = false });

                    quantity = Math.Clamp(quantity, 1, item.Quantity);

                    //Usa item
                    switch (item)
                    {
                        case WafclastFood wf:
                            player.Character.LifePoints.Add(wf.LifeGain * quantity);
                            item.Quantity -= quantity;
                            break;
                        default:
                            return Task.FromResult(new Response() { IsItemUsable = false });
                    }

                    if (item.Quantity == 0)
                        await session.RemoveItemAsync(item);
                    else
                        await session.ReplaceItemAsync(item);
                    await player.SaveAsync();

                    return Task.FromResult(new Response());
                });
            var _response = await result;

            if (_response.IsPlayerFound == false)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            if (_response.IsItemFound == false)
            {
                await ctx.ResponderAsync($"não foi encontrado o item chamado {Formatter.Bold(itemName.Titulo())}!");
                return;
            }

            if (_response.IsItemUsable == false)
            {
                await ctx.ResponderAsync($"você não pode comer {itemName.Titulo()}!");
                return;
            }

            await ctx.ResponderAsync($"você comeu {Formatter.Bold($"{quantity} {itemName.Titulo()}")}!");
            return;
        }

        private class Response
        {
            public bool IsPlayerFound = true;
            public bool IsItemFound = true;
            public bool IsItemUsable = true;
        }
    }
}
