using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.MercadoGeral;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.MercadoGeral
{
    class SeeSellsCommand : BaseCommandModule
    {
        public DataBase banco = null;

        [Command("mgvervendas")]
        [Description("Permite ver as ordens de venda de um item no mercado geral.")]
        [Usage("mgvervendas <item>")]
        public async Task UseCommandAsync(CommandContext ctx, [RemainingText] string nameItem = null)
        {
            await ctx.TriggerTypingAsync();

            if (string.IsNullOrEmpty(nameItem))
            {
                await ctx.ResponderAsync("informe um item!");
                return;
            }

            Response response;
            using (var session = await banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var ordens = await session.FindOrdensAscendingAsync(nameItem, OrdemType.Venda);

                

                    var embed = new DiscordEmbedBuilder();
                    embed.WithColor(DiscordColor.Brown);
                    embed.WithTitle($"{nameItem}");
                    embed.WithTimestamp(DateTime.Now);

                    foreach (var ordem in ordens)
                        embed.AddField($"`{ordem.Id}`", $"`{ordem.Quantidade}` ainda disponível.  {Emojis.Coins} `{ordem.Preco:N0}` cada.");

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
