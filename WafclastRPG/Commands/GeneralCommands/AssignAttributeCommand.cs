using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Maps;
using WafclastRPG.Enums;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class AssignAttributeCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("evoluir-atributo")]
        [Description("Permite atribuir pontos de atributos")]
        [Usage("evoluir-atributo")]
        public async Task AssignAttributeCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await database.FindAsync(ctx.User);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var map = await database.FindAsync(player.Character.Localization);
            if (map.Tipo != MapType.Cidade)
            {
                await ctx.ResponderAsync(Strings.SomenteNaCidade);
                return;
            }

            if (player.Character.AttributePoints == 0)
            {
                await ctx.ResponderAsync("você não tem pontos de atributos livres! Evolua o seu personagem antes.");
                return;
            }

            var quantityResponse = await ctx.WaitForIntAsync("Quantos pontos você deseja atribuir? Min 1.", database, minValue: 1);
            if (quantityResponse.TimedOut)
                return;

            if (quantityResponse.Value > player.Character.AttributePoints)
            {
                await ctx.ResponderAsync($"você não tem {quantityResponse.Value} pontos disponível!");
                return;
            }

            database.StartExecutingInteractivity(ctx);

            var embed = new DiscordEmbedBuilder();
            embed.WithDescription("Qual atributo você deseja evoluir?\n" +
                $"{Formatter.InlineCode("Força")} - {Formatter.InlineCode("Dextreza")} - " +
                $"{Formatter.InlineCode("Inteligencia")}");
            embed.WithFooter("Digite o nome do atributo ou 'sair' para fechar");

            bool continueLoop = true;
            string attribute = "";
            while (continueLoop)
            {
                var wait = await ctx.WaitForMessageAsync(ctx.User.Mention, embed.Build());
                if (wait.TimedOut)
                {
                    await ctx.ResponderAsync("tempo de resposta expirado!");
                    database.StopExecutingInteractivity(ctx);
                    return;
                }

                var response = wait.Result.Content.ToLower().Trim();

                if (response == "sair")
                {
                    database.StopExecutingInteractivity(ctx);
                    return;
                }

                response = response.RemoverAcentos();

                switch (response)
                {
                    case "forca":
                        player.Character.Strength.BaseValue += quantityResponse.Value;
                        player.Character.Strength.Restart();

                        player.Character.Life.BaseValue += quantityResponse.Value * 0.5M;
                        player.Character.PhysicalDamage.MultValue += quantityResponse.Value * 0.2M;
                        player.Character.PhysicalDamage.Restart();

                        attribute = "força";
                        continueLoop = false;
                        break;

                    case "destreza":
                        player.Character.Dexterity.BaseValue += quantityResponse.Value;
                        player.Character.Dexterity.Restart();

                        player.Character.Accuracy.BaseValue += quantityResponse.Value * 2;
                        player.Character.Accuracy.Restart();
                        player.Character.Evasion.MultValue += quantityResponse.Value * 0.2M;
                        player.Character.Evasion.Restart();

                        attribute = "dextreza";
                        continueLoop = false;
                        break;

                    case "inteligencia":
                        player.Character.Intelligence.BaseValue += quantityResponse.Value;
                        player.Character.Intelligence.Restart();

                        player.Character.Mana.BaseValue += quantityResponse.Value * 0.5M;
                        player.Character.ManaRegen = new WafclastStatePoints(player.Character.Mana.MaxValue * 0.08M);
                        player.Character.EnergyShield.MultValue += quantityResponse.Value * 0.2M;

                        attribute = "inteligencia";
                        continueLoop = false;
                        break;
                }
            }
            player.Character.AttributePoints -= quantityResponse.Value;
            database.StopExecutingInteractivity(ctx);
            await database.ReplaceAsync(player);
            await ctx.ResponderAsync($"você atribuiu {Formatter.Bold(quantityResponse.Value.ToString())} em {attribute.Titulo()}");
        }
    }
}
