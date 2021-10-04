// This file is part of the WafclastRPG project.

using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using WafclastRPG.Attributes;
using WafclastRPG.Database.Repositories;
using WafclastRPG.Database.Response;
using WafclastRPG.Extensions;
using WafclastRPG.Game.Characters;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Commands.UserCommands
{
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class AttributesCommand : BaseCommandModule
    {
        private IResponse _res;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMongoSession _session;

        public AttributesCommand(IPlayerRepository playerRepository, IRoomRepository roomRepository, IMongoSession session)
        {
            this._playerRepository = playerRepository;
            this._roomRepository = roomRepository;
            this._session = session;
        }

        [Command("atribuir")]
        [Description("Permite atribuir pontos de atributos nos atributos.")]
        [Usage("atribuir < quantidade > < atributo > ")]
        public async Task AllocateAttributesCommandAsync(CommandContext ctx, int quantity = 1, [RemainingText] string attribute = "")
        {
            using (await this._session.StartSessionAsync())
                this._res = await this._session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await this._playerRepository.FindPlayerAsync(ctx);

                    if (string.IsNullOrEmpty(attribute))
                        return new StringResponse($"você precisa informar um atributo.");

                    var character = player.Character;

                    quantity = Math.Clamp(quantity, 1, character.AttributePoints);
                    character.AttributePoints -= quantity;
                    attribute = attribute.ToLower().RemoverAcentos();

                    var atribut = this.AttributeChoose(attribute, character);
                    if (atribut == null)
                        return new StringResponse("este atributo não existe!");

                    atribut.Base += quantity;

                    await this._playerRepository.SavePlayerAsync(player);
                    return new StringResponse("pontos atribuidos!");
                });
            await ctx.RespondAsync(this._res);
        }

        public WafclastAttribute AttributeChoose(string mensagem, WafclastBaseCharacter character) =>
          mensagem switch
          {
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
        public async Task AttributesComandAsync(CommandContext ctx)
        {
            using (await this._session.StartSessionAsync())
                this._res = await this._session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await this._playerRepository.FindPlayerAsync(ctx);

                    var embed = new DiscordEmbedBuilder();
                    foreach (var item in player.Character.Attributes.GetAttributes())
                        embed.AddField(item.Name, item.Attribute.Current.ToString(), true);
                    return new EmbedResponse(embed);
                });
            await ctx.RespondAsync(this._res);
        }
    }
}
