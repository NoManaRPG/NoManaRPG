// This file is part of the WafclastRPG project.

using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using WafclastRPG.Attributes;
using WafclastRPG.Database;
using WafclastRPG.Database.Exceptions;
using WafclastRPG.Database.Repositories;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class LookAroundCommand : BaseCommandModule
    {
        private IResponse _res;
        private readonly MongoDbContext _mongoDbContext;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMongoSession _session;

        public LookAroundCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, MongoDbContext mongoDbContext, IMongoSession session)
        {
            this._playerRepository = playerRepository;
            this._mongoDbContext = mongoDbContext;
            this._roomRepository = roomRepository;
            this._session = session;
        }

        [Command("olhar")]
        [Aliases("look", "lookaround")]
        [Description("Permite olhar ao seu redor.")]
        [Usage("olhar")]
        public async Task LookAroundCommandAsync(CommandContext ctx)
        {
            var player = await this._mongoDbContext.Players.Find(x => x.Id == ctx.User.Id).FirstOrDefaultAsync();
            if (player == null)
                throw new PlayerNotCreatedException();

            var room = await this._mongoDbContext.Rooms.Find(x => x.Id == player.Character.Room.Id).FirstOrDefaultAsync();
            if (room == null)
            {
                await CommandContextExtension.RespondAsync(ctx, "parece que aconteceu algum erro na matrix!");
                return;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithColor(DiscordColor.Blue);
            embed.WithTitle(room.Name);
            embed.WithDescription(room.Description);

            var playerLoca = player.Character.Room.Location;

            var sf = new StringBuilder();
            var asd = await this._mongoDbContext.Rooms.Find(x => x.Location.X >= playerLoca.X - 160 && x.Location.X <= playerLoca.X + 160
                                                  && x.Location.Y >= playerLoca.Y - 160 && x.Location.Y <= playerLoca.Y + 160).ToListAsync();
            var showing = 0;
            foreach (var item in asd)
            {
                var distance = playerLoca.Distance(item.Location);
                if (distance != 0 && distance <= 165)
                {
                    sf.AppendLine($"{item.Name} - Distancia de {playerLoca.Distance(item.Location):N2} Km.");
                    showing++;
                }
            }
            var locations = sf.ToString();
            if (string.IsNullOrWhiteSpace(locations))
                embed.AddField("Locais proximos", "Não tem.");
            else
                embed.AddField("Locais proximos", sf.ToString());

            embed.WithFooter($"Exibindo {showing} locais próximos de {asd.Count}.");

            await ctx.RespondAsync(embed.Build());
        }


        [Command("viajar")]
        [Aliases("v", "travel")]
        [Description("Permite viajar para outro local.")]
        [Usage("viajar [ nome ]")]
        [Cooldown(1, 5, CooldownBucketType.User)]
        public async Task TravelCommandAsync(CommandContext ctx, [RemainingText] string roomName)
        {
            using (await this._session.StartSessionAsync())
                this._res = await this._session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await this._playerRepository.FindPlayerAsync(ctx);

                    var character = player.Character;

                    WafclastRoom room = null;

                    if (string.IsNullOrWhiteSpace(roomName))
                    {
                        room = await this._roomRepository.FindRoomOrDefaultAsync(ctx.Channel.Id);
                        if (room == null)
                            return new StringResponse("você foi para algum lugar, talvez alguns passos a frente.");
                    }
                    else
                    {
                        room = await this._roomRepository.FindRoomOrDefaultAsync(roomName);
                        if (room == null)
                            return new StringResponse("você tenta procurar no mapa o lugar, mas não encontra! Como você chegaria em um lugar em que você não conhece?!");
                    }

                    if (room.Location.Distance(character.Room.Location) > 161)
                        return new StringResponse("parece ser um caminho muito longe! Melhor tentar algo mais próximo.");
                    if (room == player.Character.Room)
                        return new StringResponse("como é bom estar no lugar que você sempre quis...");

                    character.Room = room;
                    await this._playerRepository.SavePlayerAsync(player);

                    return new StringResponse($"você chegou em: **[{room.Name}]!**");
                });
            await ctx.RespondAsync(this._res);
        }
    }
}
