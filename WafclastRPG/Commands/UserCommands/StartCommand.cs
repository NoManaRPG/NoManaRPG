// This file is part of the WafclastRPG project.

using System.Configuration;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using WafclastRPG.Attributes;
using WafclastRPG.Database.Interfaces;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Characters;
using WafclastRPG.Game.Entities;
using WafclastRPG.Game.Entities.Rooms;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class StartCommand : BaseCommandModule
    {
        private IResponse _res;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMongoSession _session;

        public StartCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, IMongoSession session)
        {
            this._playerRepository = playerRepository;
            this._roomRepository = roomRepository;
            this._session = session;
        }


        [Command("comecar")]
        [Aliases("start")]
        [Description("Permite criar um personagem, após informar uma classe.")]
        [Usage("comecar")]
        public async Task StartCommandAsync(CommandContext ctx, [RemainingText] string character = "")
        {
            using (await this._session.StartSessionAsync())
                this._res = await this._session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await this._playerRepository.FindPlayerOrDefaultAsync(ctx);
                    if (player != null)
                        return new StringResponse("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");

                    character = character.ToLower();
                    switch (character)
                    {
                        case "guerreiro":
                            player = new WafclastPlayer(ctx.User.Id, new WafclastCharacterWarrior());
                            break;
                        case "feitiçeira":
                        case "feiticeira":
                            player = new WafclastPlayer(ctx.User.Id, new WafclastCharacterMage());
                            break;
                        default:
                            return new StringResponse("você esqueceu de informar a classe do seu personagem! **Guerreiro ou Feiticeira.**");
                    }
                    var firstRoom = ulong.Parse(ConfigurationManager.AppSettings.Get("FirstRoomUlong"));
                    player.Character.Room = new WafclastBaseRoom(await this._roomRepository.FindRoomOrDefaultAsync(firstRoom));

                    await this._playerRepository.SavePlayerAsync(player);
                    return new StringResponse("personagem criado com sucesso! Obrigado por escolher Wafclast! Para continuar, digite `w.olhar`");
                });
            await ctx.RespondAsync(this._res);
        }
    }
}
