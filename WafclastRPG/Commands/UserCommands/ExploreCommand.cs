using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class ExploreCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("explorar")]
        [Aliases("ex")]
        [Description("Permite explorar por monstros no andar atual. Também serve para fugir de um monstro.")]
        [Usage("explorar")]
        [Cooldown(1, 15, CooldownBucketType.User)]
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

                    if (player.Character.CurrentFloor == 0)
                        return new Response("você procura na cidade toda, mas não encontra nenhum monstro.. talvez seja melhor subir alguns andares na Torre.", player.Reminder);

                    var rd = new Random();

                    var monster = await player.GetNewMonsterAsync();

                    int floorDifference = (player.Character.CurrentFloor + 1) - monster.FloorLevel;

                    monster.PhysicalDamage = new WafclastStatePoints(rd.Sortear(monster.PhysicalDamage.BaseValue, monster.PhysicalDamage.BaseValue * floorDifference));
                    monster.Evasion = new WafclastStatePoints(rd.Sortear(monster.Evasion.BaseValue, monster.Evasion.BaseValue * floorDifference));
                    monster.Accuracy = new WafclastStatePoints(rd.Sortear(monster.Accuracy.BaseValue, monster.Accuracy.BaseValue * floorDifference));
                    monster.Armour = new WafclastStatePoints(rd.Sortear(monster.Armour.BaseValue, monster.Armour.BaseValue * floorDifference));
                    monster.Life = new WafclastStatePoints(rd.Sortear(monster.Life.BaseValue, monster.Life.BaseValue * floorDifference));

                    foreach (var item in monster.DropChances)
                    {
                        double increment = (rd.Sortear(1, floorDifference) / 100) + 1;
                        item.Chance *= increment;
                    }

                    player.Character.Monster = monster;

                    await player.SaveAsync();

                    return new Response($"você encontrou um {monster.Name}.", player.Reminder);
                });

            await ctx.ResponderAsync(response.Message);

            if (response.Reminder)
            {
                await Task.Delay(15000);
                await ctx.ResponderAsync($"{Messages.Reminder} `explorar`");
            }
        }
    }
}
