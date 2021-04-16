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
    public class CookCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("cozinhar")]
        [Aliases("cook")]
        [Description("Permite cozinhar itens do tipo comida.")]
        [Usage("cozinhar <quantidade> <nome>")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task UseCommandAsync(CommandContext ctx, int quantidade, [RemainingText] string nameItem)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    if (quantidade <= 0)
                        return new Response("quantidade precisa ser maior que 0.");

                    var item = await player.GetItemAsync(nameItem);
                    if (item == null)
                        return new Response("não existe este item no seu inventário.");

                    switch (item)
                    {
                        case WafclastRawFoodItem rf:

                            if (player.Character.CookingSkill.Level < rf.CookingLevel)
                                return new Response("você não tem nível o suficiente para cozinhar este item.");

                            if (rf.Quantity < quantidade)
                                return new Response($"você não tem {quantidade} {item.Name}.");

                            var rd = new Random();
                            var chance = rf.Chance * (((double)player.Character.CookingSkill.Level / 100d) + 1);
                            var quantityCooked = 0;
                            var quantityFail = 0;

                            for (int i = 0; i < quantidade; i++)
                            {
                                if (rd.Chance(chance))
                                {
                                    quantityCooked += 1;
                                    player.Character.CookingSkill.AddExperience(rf.ExperienceGain);
                                }
                                else
                                {
                                    quantityFail += 1;
                                    player.Character.CookingSkill.AddExperience(5);
                                }
                            }

                            rf.Quantity -= quantidade;

                            if (quantityCooked >= 1)
                            {
                                var itemCooked = await session.FindItemAsync(rf.CookedItemId);
                                await player.AddItemAsync(itemCooked, quantityCooked);
                            }

                            if (rf.Quantity == 0)
                                await session.RemoveAsync(rf);
                            else
                                await session.ReplaceAsync(rf);
                            await session.ReplaceAsync(player);

                            return new Response($"você cozinhou {quantityCooked} {item.Name} e queimou {quantityFail}.");

                        default:
                            return new Response("você não pode cozinhar este item!");
                    }
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
