using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Wafclast;
using WafclastRPG.Extensions;
using WafclastRPG.Repositories.Interfaces;

namespace WafclastRPG.Commands.UserCommands {
  [ModuleLifespan(ModuleLifespan.Transient)]
  public class AttributesCommand : BaseCommandModule {
    private Response _res;
    private readonly IPlayerRepository _playerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMongoSession _session;

    public AttributesCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, IMongoSession session) {
      _playerRepository = playerRepository;
      _roomRepository = roomRepository;
      _session = session;
    }

    [Command("atribuir")]
    [Description("Permite atribuir pontos de atributos nos atributos.")]
    [Usage("atribuir < quantidade > < atributo > ")]
    public async Task AllocateAttributesCommandAsync(CommandContext ctx, int quantity = 1, [RemainingText] string attribute = "") {
      using (var session = await _session.StartSession())
        _res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          if (string.IsNullOrEmpty(attribute))
            return new Response($"você precisa informar um atributo.");

          var character = player.Character;

          quantity = Math.Clamp(quantity, 1, character.AttributePoints);
          character.AttributePoints -= quantity;
          attribute = attribute.ToLower().RemoverAcentos();

          var atribut = AttributeChoose(attribute, character);
          if (atribut == null)
            return new Response("este atributo não existe!");

          atribut.Base += quantity;

          await _playerRepository.SavePlayerAsync(player);
          return new Response("pontos atribuidos!");
        });
      await ctx.RespondAsync(_res);
    }

    public WafclastAttribute AttributeChoose(string mensagem, BaseCharacter character) =>
      mensagem switch {
        "forca" => character.Attributes.Strength,
        "constituicao" => character.Attributes.Constitution,
        "agility" => character.Attributes.Agility,
        "forca de vontade" => character.Attributes.Willpower,
        "percepcao" => character.Attributes.Perception,
        "carisma" => character.Attributes.Charisma,
        "inteligencia" => character.Attributes.Intelligence,
        "destreza" => character.Attributes.Dexterity,
        _ => null,
      };

    [Command("atributos")]
    [Description("Exibe todos os atributos e a quantia alocada de pontos.")]
    [Usage("atributos")]
    public async Task AttributesComandAsync(CommandContext ctx) {
      using (var session = await _session.StartSession())
        _res = await session.WithTransactionAsync(async (s, ct) => {
          var player = await _playerRepository.FindPlayerAsync(ctx);

          var embed = new DiscordEmbedBuilder();
          foreach (var item in player.Character.Attributes.GetAttributes())
            embed.AddField(item.Name, item.Attribute.Current.ToString(), true);
          return new Response(embed);
        });
      await ctx.RespondAsync(_res);
    }
  }
}
