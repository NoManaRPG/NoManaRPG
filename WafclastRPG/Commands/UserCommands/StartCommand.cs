using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;
using WafclastRPG.Repositories;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class StartCommand : BaseCommandModule {
    private Response _res;
    private readonly Config _config;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMongoSession _session;

    public StartCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, Config config,IMongoSession session) {
      _playerRepository = playerRepository;
      _roomRepository = roomRepository;
      _session = session;
      _config = config;
    }


    [Command("comecar")]
    [Aliases("start")]
    [Description("Permite criar um personagem, após informar uma classe.")]
    [Usage("comecar")]
    public async Task StartCommandAsync(CommandContext ctx, [RemainingText] string character = "") {
      using (var session = await _session.StartSession())
        _res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerOrDefaultAsync(ctx);
          if (player != null)
            return new Response("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");

          character = character.ToLower();
          switch (character) {
            case "guerreiro":
              player = new Player(ctx.User.Id, new CharacterWarrior());
              break;
            case "feitiçeira":
            case "feiticeira":
              player = new Player(ctx.User.Id, new MageCharacter());
              break;
            default:
              return new Response("você esqueceu de informar a classe do seu personagem! **Guerreiro ou Feiticeira.**");
          }
          ulong.TryParse(_config.FirstRoom, out ulong result);
          player.Character.Room = await _roomRepository.FindRoomOrDefaultAsync(result);

          await _playerRepository.SavePlayerAsync(player);
          return new Response("personagem criado com sucesso! Obrigado por escolher Wafclast! Para continuar, digite `w.olhar`");
        });
      await ctx.RespondAsync(_res);
    }
  }
}
