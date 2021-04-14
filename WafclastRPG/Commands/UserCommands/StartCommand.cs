using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands
{
    public class StartCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("comecar")]
        [Aliases("start")]
        [Description("Permite criar um personagem.")]
        [Usage("comecar")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task StartCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(ctx.User);
                    if (player == null)
                    {
                        player = new WafclastPlayer(ctx.User.Id);
                        await session.ReplaceAsync(player);
                        return new Response("personagem criado com sucesso! Obrigado por escolher Wafclast!");
                    }
                    return new Response("você já tem um personagem! Não é possível criar outro.");
                });
            await ctx.ResponderAsync(response.Message);
        }
    }
}
