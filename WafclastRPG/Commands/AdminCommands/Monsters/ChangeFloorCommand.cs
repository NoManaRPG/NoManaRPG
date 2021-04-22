using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.DataBases;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WafclastRPG.Commands.AdminCommands.Monsters
{
    public class ChangeFloorCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("mudarandar")]
        [Description("Permite mudar o andar de um monstro.")]
        [Usage("mudarandar <monstro> <andar>")]
        [RequireOwner]
        public async Task AddDropCommandAsync(CommandContext ctx, string monsterIdString, int andar)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {

                    if (!ObjectId.TryParse(monsterIdString, out var monsterId))
                        return new Response("o ID do monstro está inválido!");

                    var monster = await database.CollectionMonsters.Find(session.Session, x => x.Id == monsterId).FirstOrDefaultAsync();
                    if (monster == null)
                        return new Response("não encontrei este monstro, você informou o ID correto?");

                    monster.FloorLevel = andar;

                    await session.ReplaceAsync(monster);

                    return new Response($"agora {monster.Name} se encontra no andar {andar}.");
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
