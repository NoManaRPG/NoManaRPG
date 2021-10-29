// This file is part of the WafclastRPG project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using WafclastRPG.Database;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Database.Response;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Commands.AdminCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class DatabaseCommands : BaseCommandModule
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IMongoSession _session;
        private readonly UsersBlocked _usersBlocked;
        private readonly MongoDbContext _mongoDbContext;
        private readonly TimeSpan _timeout = TimeSpan.FromMinutes(2);
        private IResponse _res;

        public DatabaseCommands(IPlayerRepository playerRepository, IZoneRepository zoneRepository, IMongoSession session, MongoDbContext mongoDbContext, UsersBlocked usersBlocked)
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
            FilterDefinition<WafclastPlayer> filter = FilterDefinition<WafclastPlayer>.Empty;
            FindOptions<WafclastPlayer> options = new FindOptions<WafclastPlayer> { BatchSize = 8, NoCursorTimeout = false };
            using (IAsyncCursor<WafclastPlayer> cursor = await this._mongoDbContext.Players.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastPlayer> list = cursor.Current;
                    foreach (WafclastPlayer item in list)
                    {


                        await this._mongoDbContext.Players.ReplaceOneAsync(x => x.Id == item.Id, item);
                    }
                }

            await ctx.RespondAsync("Banco foi atualizado!");
        }

        [Command("atualizar-itens")]
        [RequireOwner]
        public async Task AtualizarItensAsync(CommandContext ctx)
        {
            FilterDefinition<WafclastItem> filter = FilterDefinition<WafclastItem>.Empty;
            FindOptions<WafclastItem> options = new FindOptions<WafclastItem> { BatchSize = 8, NoCursorTimeout = false };
            using (IAsyncCursor<WafclastItem> cursor = await this._mongoDbContext.Items.FindAsync(filter, options))
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<WafclastItem> list = cursor.Current;
                    foreach (WafclastItem item in list)
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
