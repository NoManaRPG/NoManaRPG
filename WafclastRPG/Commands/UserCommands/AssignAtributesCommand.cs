using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class AssignAtributesCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("atribuir")]
        [Description("Permite atribuir os pontos de atributos do seu personagem")]
        [Usage("atribuir <quantidade> <atributo>")]
        [Cooldown(1, 15, CooldownBucketType.User)]
        public async Task AssignAtributesCommandAsync(CommandContext ctx, ulong quantidade, string atributo)
        {
            await ctx.TriggerTypingAsync();


            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    if (player.Character.AttributePoints == 0)
                        return new Response("você não tem pontos para atribuir");

                    if (quantidade > player.Character.AttributePoints)
                        return new Response($"você somente tem {player.Character.AttributePoints} pontos.");



                    atributo = atributo.ToLower();
                    switch (atributo)
                    {
                        case "forca":
                        case "força":
                            player.Character.Strength.BaseValue += quantidade;
                            player.Character.Strength.Restart();

                            player.Character.Life.BaseValue += quantidade * 0.5;
                            player.Character.PhysicalDamage.MultValue += quantidade * 0.2;
                            player.Character.PhysicalDamage.Restart();

                            break;

                        case "destreza":
                            player.Character.Dexterity.BaseValue += quantidade;
                            player.Character.Dexterity.Restart();

                            player.Character.Accuracy.BaseValue += quantidade * 2;
                            player.Character.Accuracy.Restart();
                            player.Character.Evasion.MultValue += quantidade * 0.2;
                            player.Character.Evasion.Restart();
                            break;

                        case "inteligencia":
                            player.Character.Intelligence.BaseValue += quantidade;
                            player.Character.Intelligence.Restart();

                            player.Character.Mana.BaseValue += quantidade * 0.5;
                            player.Character.ManaRegen = new WafclastStatePoints(player.Character.Mana.MaxValue * 0.08);
                            player.Character.EnergyShield.MultValue += quantidade * 0.2;

                            break;

                        default:
                            return new Response("você informou um atributo inválido.");
                    }
                    player.Character.AttributePoints -= quantidade;

                    await player.SaveAsync();

                    return new Response($"você atribuiu {quantidade} pontos em {atributo}!");
                });

            await ctx.ResponderAsync(response.Message);
        }
    }
}
