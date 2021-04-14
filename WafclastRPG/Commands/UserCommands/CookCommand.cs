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
        public async Task UseCommandAsync(CommandContext ctx, int quantidade, [RemainingText] string itemNome)
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

                    var item = await session.FindAsync(itemNome, player);
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
                            var chance = rf.Chance * (player.Character.CookingSkill.Level / 100) + 1;
                            var cooked = 0;
                            var fail = 0;

                            for (int i = 0; i < quantidade; i++)
                            {
                                if (rd.Chance(chance))
                                {
                                    cooked += 1;
                                    player.Character.CookingSkill.AddExperience(rf.ExperienceGain);
                                }
                                else
                                {
                                    fail += 1;
                                    player.Character.CookingSkill.AddExperience(5);
                                }
                            }

                            rf.Quantity -= quantidade;

                            if (cooked >= 1)
                            {
                                var itemCooked = await session.FindItemAsync(rf.CookedItemId);
                                await session.InsertAsync(item, cooked, player);
                            }
                            if (rf.Quantity == 0)
                                await session.RemoveAsync(rf);
                            else
                                await session.ReplaceAsync(rf);
                            await session.ReplaceAsync(player);

                            return new Response($"você cozinhou {cooked} {item.Name} e queimou {fail}.");

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
