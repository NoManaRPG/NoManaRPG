using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class SeeCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("olhar")]
        [Aliases("see")]
        [Description("Permite olhar para algum objeto ou pessoa.")]
        [Usage("olhar [ item ]")]
        public async Task SeeCommandAsync(CommandContext ctx, [RemainingText] string nameItem = "")
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    var reg = await player.GetRegionAsync();
                    if (reg == null)
                        return new Response("Região não encontrada! Parece que aconteceu um bug.");

                    var embed = new DiscordEmbedBuilder();
                    embed.WithDescription($"Ao olhar em volta, você percebe que está em {reg.Name}.\n\n{reg.Description}.");
                    return new Response(embed);
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }
    }
}