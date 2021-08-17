using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    class SaquearCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("saquear")]
        [Aliases("loot")]
        [Description("Permite saquear um monstro morto.")]
        [Usage("saquear")]
        public async Task SaquearCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var cha = player.Character;
                    var target = cha.CurrentFightingMonster;

                    if (target == null)
                        return new Response($"você não eliminou nenhum monstro para saquear!");
                    else if (target.LifePoints > 0)
                        return new Response($"{target.Name} ainda está vivo!");

                    //Combat
                    var rd = new Random();
                    var str = new StringBuilder();
                    var embed = new DiscordEmbedBuilder();


                    embed.WithColor(DiscordColor.Red);
                    embed.WithAuthor($"{ctx.User.Username}", iconUrl: ctx.User.AvatarUrl);
                    embed.WithTitle($"Loot");

                    foreach (var drop in target.Drops)
                    {
                        if (rd.Chance(drop.Chance))
                        {
                            var item = await session.FindItemAsync(drop.GlobalItemId, ctx.Client.CurrentUser);
                            if (item == null)
                            {
                                ctx.Client.Logger.LogInformation(new EventId(608, "ERROR"), $"{target.Name} está com o drop {drop.GlobalItemId} errado!", DateTime.Now);
                                continue;
                            }

                            var quantity = Convert.ToUInt64(rd.Sortear(drop.MinQuantity, drop.MaxQuantity));

                            str.AppendLine($"**+ {quantity} {item.Name.Title()}.**");
                            cha.Inventory.Add(item);
                        }
                    }


                    var embed = new DiscordEmbedBuilder();
                    embed.WithDescription($"Ao olhar em volta");
                    return new Response(embed);
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }
    }
}
