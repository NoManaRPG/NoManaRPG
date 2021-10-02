// This file is part of the WafclastRPG project.

using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using WafclastRPG.Attributes;
using WafclastRPG.Database.Repositories;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Characters;
using WafclastRPG.Game.Entities.Wafclast;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class StartCommand : BaseCommandModule
    {
        private IResponse _res;
        private readonly Config _config;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMongoSession _session;

        public StartCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, Config config, IMongoSession session)
        {
            this._playerRepository = playerRepository;
            this._roomRepository = roomRepository;
            this._session = session;
            this._config = config;
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
                            player = new Player(ctx.User.Id, new CharacterWarrior());
                            break;
                        case "feitiçeira":
                        case "feiticeira":
                            player = new Player(ctx.User.Id, new MageCharacter());
                            break;
                        default:
                            return new StringResponse("você esqueceu de informar a classe do seu personagem! **Guerreiro ou Feiticeira.**");
                    }
                    ulong.TryParse(this._config.FirstRoom, out ulong result);
                    player.Character.Room = await this._roomRepository.FindRoomOrDefaultAsync(result);

                    await this._playerRepository.SavePlayerAsync(player);
                    return new StringResponse("personagem criado com sucesso! Obrigado por escolher Wafclast! Para continuar, digite `w.olhar`");
                });
            await ctx.RespondAsync(this._res);
        }
    }
}
