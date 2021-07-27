using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class ExploreCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("explorar")]
        [Aliases("ex")]
        [Description("Permite explorar a região.")]
        [Usage("explorar")]
        public async Task ExploreCommandAsync(CommandContext ctx)
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

                    Random rnd = new Random();
                    int r = rnd.Next(reg.Monsters.Count);
                    player.Character.CurrentFightingMonster = reg.Monsters[r];

                    await player.SaveAsync();

                    return new Response($"você encontrou: {reg.Monsters[r].Name}!");
                });

            await ctx.ResponderAsync(response.Message);
        }
    }
}
