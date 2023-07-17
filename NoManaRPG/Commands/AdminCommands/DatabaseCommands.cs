// This file is part of WafclastRPG project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using NoManaRPG.Database;
using NoManaRPG.Database.Repositories;
using NoManaRPG.Database.Response;
using NoManaRPG.Game.Entities;
using NoManaRPG.Game.Entities.Items;

namespace NoManaRPG.Commands.AdminCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class DatabaseCommands : BaseCommandModule
    {
        private readonly PlayerRepository _playerRepository;
        private readonly ZoneRepository _zoneRepository;
        private readonly MongoSession _session;
        private readonly UsersBlocked _usersBlocked;
        private readonly MongoDbContext _mongoDbContext;
        private readonly TimeSpan _timeout = TimeSpan.FromMinutes(2);
        private IResponse _res;

        public DatabaseCommands(PlayerRepository playerRepository, ZoneRepository zoneRepository, MongoSession session, MongoDbContext mongoDbContext, UsersBlocked usersBlocked)
        {
            this._playerRepository = playerRepository;
            this._zoneRepository = zoneRepository;
            this._session = session;
            this._usersBlocked = usersBlocked;
            this._mongoDbContext = mongoDbContext;
        }

        [Command("atualizar-jogadores")]
        [RequireOwner]
        public async Task AtualizarAsync(CommandContext ctx)
        {
            FilterDefinition<Player> filter = FilterDefinition<Player>.Empty;
            FindOptions<Player> options = new FindOptions<Player> { BatchSize = 8, NoCursorTimeout = false };
            using (IAsyncCursor<Player> cursor = await this._mongoDbContext.Players.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<Player> list = cursor.Current;
                    foreach (Player item in list)
                    {


                        await this._mongoDbContext.Players.ReplaceOneAsync(x => x.DiscordId == item.DiscordId, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("atualizar-itens")]
        [RequireOwner]
        public async Task AtualizarItensAsync(CommandContext ctx)
        {
            FilterDefinition<Item> filter = FilterDefinition<Item>.Empty;
            FindOptions<Item> options = new FindOptions<Item> { BatchSize = 8, NoCursorTimeout = false };
            using (IAsyncCursor<Item> cursor = await this._mongoDbContext.Items.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<Item> list = cursor.Current;
                    foreach (Item item in list)
                    {


                        await this._mongoDbContext.Items.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }
            await ctx.RespondAsync("Banco atualizado!");
        }

        [Command("sudo")]
        [RequireOwner]
        public async Task SudoAsync(CommandContext ctx, DiscordUser member, [RemainingText] string command)
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

        //[Command("aviajar")]
        //[Aliases("av", "atravel")]
        //[Hidden]
        //[RequireOwner]
        //public async Task AdminTravelCommandAsync(CommandContext ctx, [RemainingText] string roomName) {
        //  using (var session = await Data.StartDatabaseSessionAsync())
        //    Res = await session.WithTransactionAsync(async (s, ct) => {
        //      var player = await session.FindPlayerAsync(ctx);

        //      var character = player.Character;
        //      Room room = null;

        //      if (string.IsNullOrWhiteSpace(roomName)) {
        //        room = await session.FindRoomAsync(ctx.Channel.Id);
        //        if (room == null)
        //          return new Response("você foi para algum lugar, talvez alguns passos a frente.");
        //      } else {
        //        room = await session.FindRoomAsync(roomName);
        //        if (room == null)
        //          return new Response("você tenta procurar no mapa o lugar, mas não encontra! Como você chegaria em um lugar em que você não conhece?!");
        //      }

        //      room.Monster = null;
        //      character.Room = room;
        //      await player.SaveAsync();

        //      return new Response($"você chegou em: **[{room.Name}]!**");
        //    });
        //  await ctx.ResponderAsync(Res);
        //}
    }
}
