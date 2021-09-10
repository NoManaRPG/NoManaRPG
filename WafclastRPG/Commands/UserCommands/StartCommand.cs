using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Characters;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands {
  public class StartCommand : BaseCommandModule {
    public DataBase database;

    [Command("comecar")]
    [Aliases("start")]
    [Description("Permite criar um personagem, após informar uma classe.")]
    [Usage("comecar")]
    public async Task StartCommandAsync(CommandContext ctx, [RemainingText] string character = "") {
      await ctx.TriggerTypingAsync();

      Response response;
      using (var session = await database.StartDatabaseSessionAsync())
        response = await session.WithTransactionAsync(async (s, ct) => {
          var player = await session.FindPlayerAsync(ctx.User);
          if (player != null)
            return new Response("você já criou um personagem! Se estiver com dúvidas ou problemas, consulte o nosso Servidor Oficial do Discord.");

          character = character.ToLower();
          switch (character) {
            case "guerreiro":
              player = new WafclastPlayer(ctx.User.Id, new WafclastWarrior());
              break;
            case "feitiçeira":
            case "feiticeira":
              player = new WafclastPlayer(ctx.User.Id, new WafclastMage());
              break;
            default:
              return new Response("você esqueceu de informar a classe do seu personagem! **Guerreiro ou Feiticeira.**");
          }

          player.Character.Region = await session.FindRegionAsync(1);

          await session.ReplaceAsync(player);
          return new Response("personagem criado com sucesso! Obrigado por escolher Wafclast! Para continuar, digite `w.olhar`");
        });

      await ctx.ResponderAsync(response.Message);
    }
  }
}
