using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Bson;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class StartCommand : BaseCommandModule {
    public Response Res { private get; set; }
    public DataBase Data { private get; set; }
    public Config Config { private get; set; }

    [Command("comecar")]
    [Aliases("start")]
    [Description("Permite criar um personagem, após informar uma classe.")]
    [Usage("comecar")]
    public async Task StartCommandAsync(CommandContext ctx, [RemainingText] string character = "") {
      using (var session = await Data.StartDatabaseSessionAsync())
        Res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx, false);
          if (player != null)
            return new Response("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");

          character = character.ToLower();
          switch (character) {
            case "guerreiro":
              player = new Player(ctx.User.Id, new CharacterWarrior());
              break;
            case "feitiçeira":
            case "feiticeira":
              player = new Player(ctx.User.Id, new CharacterMage());
              break;
            default:
              return new Response("você esqueceu de informar a classe do seu personagem! **Guerreiro ou Feiticeira.**");
          }
          ulong.TryParse(Config.FirstRoom, out ulong result);
          player.Character.Room = await session.FindRoomAsync(result);

          await session.ReplaceAsync(player);
          return new Response("personagem criado com sucesso! Obrigado por escolher Wafclast! Para continuar, digite `w.olhar`");
        });
      await ctx.ResponderAsync(Res);
    }
  }
}
