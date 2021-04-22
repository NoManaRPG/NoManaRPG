using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.Skills
{
    public class CookCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("cozinhar")]
        [Aliases("cook")]
        [Description("Permite cozinhar itens do tipo comida.")]
        [Usage("cozinhar <quantidade> <nome>")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task UseCommandAsync(CommandContext ctx, ulong quantidade, [RemainingText] string nameItem)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    if (quantidade == 0)
                        return new Response("quantidade precisa ser maior que 0.");

                    var nameItemCook = nameItem.Replace("cru", "cozido", StringComparison.InvariantCultureIgnoreCase);

                    var fab = await session.FindFabricationAsync(nameItemCook);
                    if (fab == null)
                        return new Response("você não pode cozinhar este item!");

                    if (player.Character.CookingSkill.Level < fab.RequiredLevel)
                        return new Response("você não tem nível em culinária o suficiente.");

                    foreach (var reqItem in fab.RequiredItems)
                    {
                        var playerItem = await player.GetItemAsync(reqItem.Name);
                        if (playerItem == null || (reqItem.Quantity * quantidade) > playerItem.Quantity)
                            return new Response($"você precisa de {reqItem.Quantity * quantidade} x {reqItem.Name} para cozinhar este item.");

                        playerItem.Quantity -= reqItem.Quantity * quantidade;
                        await player.SaveItemAsync(playerItem);
                    }

                    var rd = new Random();
                    var cookLevel = player.Character.CookingSkill.Level;
                    var foodLevel = fab.RequiredLevel;
                    var par1 = cookLevel - foodLevel;
                    var par2 = par1 / 20d;
                    var levelDifference = par2 + 1;
                    var chance = fab.Chance * levelDifference;

                    ulong quantityCooked = 0;
                    var quantityFail = 0;
                    double expGain = 0;

                    for (ulong i = 0; i < quantidade; i++)
                    {
                        if (rd.Chance(chance))
                        {
                            quantityCooked += 1;
                            expGain += fab.Experience;
                        }
                        else
                        {
                            quantityFail += 1;
                            expGain += 5;
                        }
                    }

                    if (quantityCooked >= 1)
                    {
                        var itemCooked = await session.FindItemAsync(fab.Name, ctx.Client.CurrentUser);
                        await player.AddItemAsync(itemCooked, quantityCooked);
                    }

                    player.Character.CookingSkill.AddExperience(expGain);
                    await player.SaveAsync();

                    var embed = new DiscordEmbedBuilder();
                    embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
                    embed.WithTitle($"Cozinhando {nameItem}.");
                    embed.WithDescription($"+{expGain} {Emojis.Exp}");
                    embed.AddField($"Você cozinhou", $"{quantityCooked} :poultry_leg:");
                    embed.AddField($"Você queimou", $"{quantityFail} :fire:");
                    embed.WithColor(DiscordColor.Brown);
                    embed.WithTimestamp(DateTime.Now);

                    return new Response(embed);
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
