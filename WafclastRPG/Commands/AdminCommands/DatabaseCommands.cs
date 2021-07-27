using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WafclastRPG.Entities;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Itens;
using WafclastRPG.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace WafclastRPG.Commands.AdminCommands
{
    public class DatabaseCommands : BaseCommandModule
    {
        public DataBase database;

        [Command("atualizar-jogadores")]
        [RequireOwner]
        public async Task AtualizarAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastPlayer> filter = FilterDefinition<WafclastPlayer>.Empty;
            FindOptions<WafclastPlayer> options = new FindOptions<WafclastPlayer>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastPlayer> cursor = await database.CollectionPlayers.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastPlayer> list = cursor.Current;

                    foreach (WafclastPlayer item in list)
                    {


                        //item.Character.ExperienceForNextLevel = item.Character.ExperienceTotalLevel(2);


                        await database.CollectionPlayers.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("atualizar-itens")]
        [RequireOwner]
        public async Task AtualizarItensAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastBaseItem> filter = FilterDefinition<WafclastBaseItem>.Empty;
            FindOptions<WafclastBaseItem> options = new FindOptions<WafclastBaseItem>
            {
                BatchSize = 8,
                NoCursorTimeout = false
            };

            using (IAsyncCursor<WafclastBaseItem> cursor = await database.CollectionItems.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastBaseItem> list = cursor.Current;

                    foreach (WafclastBaseItem item in list)
                    {
                        WafclastCookedFoodItem cook = null;
                        switch (item.Name)
                        {
                            case "Bife Cozido":
                            case "Carne de Coelho Cozido":
                            case "Galinha Cozido":
                                cook = new WafclastCookedFoodItem(item)
                                {
                                    LifeGain = 50
                                };
                                break;
                        }
                        if (cook != null)
                            await database.CollectionItems.ReplaceOneAsync(x => x.Id == item.Id, cook);
                    }
                }
            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("sudo")]
        [RequireOwner]
        public async Task Sudo(CommandContext ctx, DiscordUser member, [RemainingText] string command)
        {
            await ctx.TriggerTypingAsync();
            var cmd = ctx.CommandsNext.FindCommand(command, out var args);
            if (cmd == null)
            {
                await ctx.RespondAsync("Comando não encontrado");
                return;
            }

            var cfx = ctx.CommandsNext.CreateFakeContext(member, ctx.Channel, "", "w.", cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        }

        [Command("restartdb")]
        [RequireOwner]
        public async Task RestartDb(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription("Você está preste a apagar todo o banco de dados! Você tem certeza dessa operação?");

            var booleano = await ctx.WaitForBoolAsync(database, embed.Build());
            if (booleano)
            {
                await database.Client.DropDatabaseAsync("WafclastV2Debug");
                await ctx.ResponderAsync("Operação executada! Reinicie o cliente.");
            }
        }

        [Command("replaceregions")]
        [RequireOwner]
        public async Task ReplaceRegions(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription("Você está preste a atualizar todas as regiões! Você tem certeza dessa operação?");

            var booleano = await ctx.WaitForBoolAsync(database, embed.Build());
            if (booleano)
            {
                await database.ReplaceRegionsAsync();
                await ctx.ResponderAsync("Operação executada!");
            }
        }

        [Command("reloaditems")]
        [RequireOwner]
        public async Task ReloadItems(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            embed.WithDescription("Você está preste a atualizar todos os itens 'Base'! Você tem certeza dessa operação?");

            var booleano = await ctx.WaitForBoolAsync(database, embed.Build());
            if (booleano)
            {
                await database.ReloadItemsAsync(ctx.Client.CurrentUser.Id);
                await ctx.ResponderAsync("Operação executada!");
            }
        }

    }
}
