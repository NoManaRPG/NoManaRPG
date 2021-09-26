using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class AllocateAttributesCommand : BaseCommandModule {
    private Response _res;
    private readonly IPlayerRepository _playerRepository;

    public AllocateAttributesCommand(IPlayerRepository playerRepository) {
      _playerRepository = playerRepository;
    }

    [Command("alocar")]
    [Description("Permite alocar pontos de atributos livres.")]
    [Usage("alocar <atributo> [ quantidade ] ")]
    public async Task AllocateAttributesCommandAsync(CommandContext ctx, int quantity = 1, [RemainingText] string attribute = "") {
      using (var sessionHandler = (SessionHandler) await _playerRepository.StartSession())
        _res = await sessionHandler.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          if (string.IsNullOrEmpty(attribute))
            return new Response($"você precisa informar um atributo.");

          var character = player.Character;

          if (quantity > character.AttributePoints)
            return new Response($"{ctx.User.Mention}, você não tem essa quantia informada de pontos!");

          character.AttributePoints -= quantity;
          attribute = attribute.ToLower().RemoverAcentos();

          switch (attribute) {
            case "forca":
              character.Attributes.Strength.Base += quantity;
              break;
            case "constituicao":
              character.Attributes.Constitution.Base += quantity;
              break;
            case "agility":
              character.Attributes.Agility.Base += quantity;
              break;
            case "forca de vontade":
              character.Attributes.Willpower.Base += quantity;
              break;
            case "percepcao":
              character.Attributes.Perception.Base += quantity;
              break;
            case "carisma":
              character.Attributes.Charisma.Base += quantity;
              break;
            case "inteligencia":
              character.Attributes.Intelligence.Base += quantity;
              break;
            case "destreza":
              character.Attributes.Dexterity.Base += quantity;
              break;
            default:
              return new Response("este atributo não existe!");
          }

          await _playerRepository.SavePlayerAsync(player);
          return new Response("pontos atribuidos!");
        });
      await ctx.ResponderAsync(_res);
    }
  }
}