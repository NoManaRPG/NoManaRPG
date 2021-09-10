﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands.CombatCommands
{
    public class ExploreCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("explorar")]
        [Aliases("ex", "explore")]
        [Description("Permite explorar uma região, podendo encontrar monstros.")]
        [Usage("explorar")]
        [CooldownAttribute(1, 5, CooldownBucketType.User)]
        public async Task ExploreCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.AindaNaoCriouPersonagem);

                    //var monster = getnew monster

                    var monster = new WafclastMonster(1, "Boneco de testes", 5, 5, 5, 5, 5);

                    player.Character.Monster = monster;

                    await player.SaveAsync();

                    return new Response($"você encontrou [{monster.Name}]!");


                });

            await ctx.ResponderAsync(response.Message);

        }
    }
}
