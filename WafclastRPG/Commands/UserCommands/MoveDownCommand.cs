using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class MoveDownCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("descer")]
        [Aliases("down")]
        [Description("Permite descer um andar na Torre.")]
        [Usage("descer")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task UseCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    if (player.Character.CurrentFloor == 0)
                        return new Response("você já está no térreo!");

                    player.Character.CurrentFloor -= 1;

                    await session.ReplaceAsync(player);

                    return new Response($"você desceu para o andar {player.Character.CurrentFloor}!");
                });

            await ctx.ResponderAsync(response.Message);
            return;
        }
    }
}
