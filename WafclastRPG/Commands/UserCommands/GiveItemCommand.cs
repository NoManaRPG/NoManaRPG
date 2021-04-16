using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class GiveItemCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("dar")]
        [Aliases("give")]
        [Description("Permite dar itens para outro jogador")]
        [Usage("dar <@jogador> <quantidade> <item>")]
        public async Task GiveItemCommandAsync(CommandContext ctx, DiscordUser jogador, int quantity, [RemainingText] string nameItem)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var target = await session.FindAsync(jogador);
                    if (target == null)
                        return new Response($"{jogador.Mention} {Messages.AindaNaoCriouPersonagem}");

                    var item = await player.GetItemAsync(nameItem);
                    if (item == null)
                        return new Response($"você não tem **{nameItem}**!");

                    quantity = Math.Abs(quantity);

                    if (quantity > item.Quantity)
                        return new Response($"você somente tem {item.Quantity}!");

                    item.Quantity -= quantity;
                    if (item.Quantity == 0)
                        await session.RemoveAsync(item);
                    else
                        await session.ReplaceAsync(item);

                    await target.AddItemAsync(item, quantity);

                    return new Response($"você deu {quantity} x {item.Name} para {jogador.Mention}!");
                });

            await ctx.ResponderAsync(response.Message);
        }
    }
}
