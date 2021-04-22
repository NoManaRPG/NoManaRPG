using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Entities.MercadoGeral;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class ExamineCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("examinar")]
        [Description("Permite examinar um item.")]
        [Usage("examinar <item>")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task ExamineCommandAsync(CommandContext ctx, [RemainingText] string itemName)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var itemGlobal = await session.FindItemAsync(itemName, ctx.Client.CurrentUser);
                    if (itemGlobal == null)
                        return new Response("o item informado não existe!");

                    var itemPlayer = await player.GetItemAsync(itemName);

                    var embed = new DiscordEmbedBuilder();
                    embed.WithAuthor("Wafclast RPG", "https://discord.gg/MAR4NFq", ctx.Client.CurrentUser.AvatarUrl);
                    embed.WithTitle(itemGlobal.Name);
                    embed.WithColor(DiscordColor.Aquamarine);
                    embed.WithDescription(Formatter.BlockCode(itemGlobal.Description));
                    embed.WithTimestamp(DateTime.Now);
                    embed.AddField("ID Global", $"`{itemGlobal.Id}`", true);
                    if (itemPlayer != null)
                        embed.AddField("ID Inventário", $"`{itemPlayer.Id}`", true);

                    if (itemGlobal.CanSell)
                    {
                        embed.AddField("Pode vender", "Sim", true);
                        var compra = await session.FindOrdensAscendingAsync(itemGlobal.Name, OrdemType.Venda, 1);
                        if (compra.Count == 1)
                            embed.AddField("Preço para compra", compra[0].Preco.ToString("N0"));
                        else
                            embed.AddField("Preço para compra", "Ninguém está vendendo.");
                    }
                    else
                        embed.AddField("Pode vender", "Não", true);

                    if (itemPlayer != null)
                        embed.AddField("Você tem", itemPlayer.Quantity.ToString("N0"), true);
                    else
                        embed.AddField("Você tem", "0", true);

                    switch (itemGlobal)
                    {
                        case WafclastMonsterCoreItem mci:
                            embed.AddField("Experiencia provida", $"{Emojis.Exp} {mci.ExperienceGain}", true);
                            break;
                        case WafclastCookedFoodItem cfi:
                            embed.AddField("Vida provida", $"{cfi.LifeGain}", true);
                            break;
                        case WafclastPickaxeItem pi:
                            embed.AddField("Dureza", $"{pi.Hardness}");
                            embed.AddField("Drop bonus", $"+{pi.DropChanceBonus}%");
                            embed.AddField("Força requisitada", $"{pi.Strength}");
                            break;
                    }
                    return new Response(embed);
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }
    }
}
