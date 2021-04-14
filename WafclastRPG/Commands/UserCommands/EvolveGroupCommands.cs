using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands
{
    [Group("evoluir")]
    [Aliases("ev")]
    [Description("Permite evoluir atributos e personagem.")]
    [Usage("evoluir [ atributos | personagem ]")]
    public class EvolveGroupCommands : BaseCommandModule
    {
        public DataBase database;

        [GroupCommand()]
        public async Task EvolveGroupCommandsAsync(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            embed.WithColor(DiscordColor.Green);

            var str = new StringBuilder();
            str.AppendLine("Evoluir é o processo de infundir núcleos com algo.");
            str.AppendLine("Para evoluir atributos digite: `evoluir atributos`");
            str.AppendLine("Para evoluir personagem digite: `evoluir personagem`");

            embed.WithDescription(str.ToString());
            await ctx.ResponderAsync(embed.Build());
        }

        [Command("atributos")]
        [Aliases("a")]
        [Description("Permite usar pontos livres para evoluir um atributo.")]
        [Usage("evoluir atributos")]
        public async Task EvolveAttributeAsync(CommandContext ctx)
        {
            //await ctx.TriggerTypingAsync();
            //var player = await database.FindAsync(ctx.User);
            //if (player.Character == null)
            //{
            //    //  await ctx.ResponderAsync(Strings.NovoJogador);
            //    return;
            //}

            //if (player.Character.AttributePoints == 0)
            //{
            //    await ctx.ResponderAsync("você não tem pontos de atributos livres! Evolua o seu personagem antes.");
            //    return;
            //}

            //var quantityResponse = await ctx.WaitForIntAsync("Quantos pontos você deseja atribuir? Min 1.", database, minValue: 1);
            //if (quantityResponse.TimedOut)
            //    return;

            //if (quantityResponse.Value > player.Character.AttributePoints)
            //{
            //    await ctx.ResponderAsync($"você não tem {quantityResponse.Value} pontos disponível!");
            //    return;
            //}

            //database.StartExecutingInteractivity(ctx);

            //var embed = new DiscordEmbedBuilder();
            //embed.WithDescription("Qual atributo você deseja evoluir?\n" +
            //    $"{Formatter.InlineCode("Força")} - {Formatter.InlineCode("Dextreza")} - " +
            //    $"{Formatter.InlineCode("Inteligencia")}");
            //embed.WithFooter("Digite o nome do atributo ou 'sair' para fechar");

            //bool continueLoop = true;
            //string attribute = "";
            //while (continueLoop)
            //{
            //    var wait = await ctx.WaitForMessageAsync(ctx.User.Mention, embed.Build());
            //    if (wait.TimedOut)
            //    {
            //        await ctx.ResponderAsync("tempo de resposta expirado!");
            //        database.StopExecutingInteractivity(ctx);
            //        return;
            //    }

            //    var response = wait.Result.Content.ToLower().Trim();

            //    if (response == "sair")
            //    {
            //        database.StopExecutingInteractivity(ctx);
            //        return;
            //    }

            //    response = response.RemoverAcentos();

            //    switch (response)
            //    {
            //        case "forca":
            //            player.Character.Strength.BaseValue += quantityResponse.Value;
            //            player.Character.Strength.Restart();

            //            player.Character.Life.BaseValue += quantityResponse.Value * 0.5M;
            //            player.Character.PhysicalDamage.MultValue += quantityResponse.Value * 0.2M;
            //            player.Character.PhysicalDamage.Restart();

            //            attribute = "força";
            //            continueLoop = false;
            //            break;

            //        case "destreza":
            //            player.Character.Dexterity.BaseValue += quantityResponse.Value;
            //            player.Character.Dexterity.Restart();

            //            player.Character.Accuracy.BaseValue += quantityResponse.Value * 2;
            //            player.Character.Accuracy.Restart();
            //            player.Character.Evasion.MultValue += quantityResponse.Value * 0.2M;
            //            player.Character.Evasion.Restart();

            //            attribute = "dextreza";
            //            continueLoop = false;
            //            break;

            //        case "inteligencia":
            //            player.Character.Intelligence.BaseValue += quantityResponse.Value;
            //            player.Character.Intelligence.Restart();

            //            player.Character.Mana.BaseValue += quantityResponse.Value * 0.5M;
            //            player.Character.ManaRegen = new WafclastStatePoints(player.Character.Mana.MaxValue * 0.08M);
            //            player.Character.EnergyShield.MultValue += quantityResponse.Value * 0.2M;

            //            attribute = "inteligencia";
            //            continueLoop = false;
            //            break;
            //    }
            //}
            //player.Character.AttributePoints -= quantityResponse.Value;
            //database.StopExecutingInteractivity(ctx);
            //await database.ReplaceAsync(player);
            //await ctx.ResponderAsync($"você atribuiu {Formatter.Bold(quantityResponse.Value.ToString())} em {attribute.Titulo()}");
        }

        [Command("personagem")]
        [Aliases("p")]
        [Description("Permite infundir núcleos para ganhar experiencia.")]
        [Usage("evoluir personagem [ quantidade ] [ nome do item ]")]
        public async Task EvolveCharacterAsync(CommandContext ctx, int quantity, [RemainingText] string itemName)
        {
            //await ctx.TriggerTypingAsync();

            //quantity = Math.Abs(quantity);

            //Response response;
            //using (var session = await database.StartDatabaseSessionAsync())
            //    response = await session.WithTransactionAsync(async (s, ct) =>
            //    {
            //        var player = await session.FindAsync(ctx.User);

            //        Thread.CurrentThread.CurrentUICulture = new CultureInfo(player.Language);

            //        if (player.Character == null)
            //            return new Response(Messages.NaoEscreveuComecar);

            //        var item = await session.FindAsync(itemName, player);
            //        if (item == null)
            //            return new Response($"não foi encontrado o item chamado {Formatter.Bold(itemName.Titulo())}!");

            //        if (quantity > item.Quantity)
            //            return new Response("você não tem toda essa quantidade para infundir!");

            //        string mensagem = "";
            //        switch (item)
            //        {
            //            case WafclastMonsterCoreItem mc:
            //                var evoluiu = player.Character.AddExperience(mc.ExperienceGain * quantity);
            //                item.Quantity -= quantity;
            //                if (evoluiu)
            //                    mensagem = $"você infundiu **{quantity}** {mc.Name.Titulo()} e ganhou `{(quantity * mc.ExperienceGain):N2}` de experiencia! Parece que você evoluiu!'";
            //                else
            //                    mensagem = $"você infundiu **{quantity}** {mc.Name.Titulo()} e ganhou `{(quantity * mc.ExperienceGain):N2}` de experiencia!";

            //                break;
            //            default:
            //                return new Response($"você não pode infundir {itemName.Titulo()}!");
            //        }

            //        if (item.Quantity == 0)
            //        {
            //            await session.RemoveAsync(item);
            //        }
            //        else
            //            await session.ReplaceAsync(item);
            //        await session.ReplaceAsync(player);

            //        return new Response(mensagem);
            //    });

            //await ctx.ResponderAsync(response.Message);
        }
    }
}
