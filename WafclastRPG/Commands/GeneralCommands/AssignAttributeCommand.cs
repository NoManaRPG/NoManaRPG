using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Enums;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class AssignAttributeCommand : BaseCommandModule
    {
        public Database database;

        [Command("evoluir-atributo")]
        [Description("Permite atribuir pontos de atributos")]
        [Usage("evoluir-atributo")]
        public async Task AssignAttributeCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await database.FindPlayerAsync(ctx);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var map = await database.FindMapAsync(player.Character.Localization.ChannelId);
            if (map.Tipo != MapType.Cidade)
            {
                await ctx.ResponderAsync(Strings.SomenteNaCidade);
                return;
            }

            if (player.Character.Atributos.PontosLivreAtributo == 0)
            {
                await ctx.ResponderAsync("você não tem pontos de atributos livres para atribuir! Evolua o seu personagem antes.");
                return;
            }

            var quantityResponse = await ctx.WaitForIntAsync("Quantos pontos você deseja atribuir? Min 1.", database, minValue: 1);
            if (quantityResponse.TimedOut)
                return;

            if (quantityResponse.Result > player.Character.Atributos.PontosLivreAtributo)
            {
                await ctx.ResponderAsync($"você não tem {quantityResponse.Result} pontos disponível!");
                return;
            }

            database.StartExecutingInteractivity(ctx);

            var embed = new DiscordEmbedBuilder();
            embed.WithDescription("Qual atributo você deseja evoluir?\n" +
                $"{Formatter.InlineCode("Força")} - {Formatter.InlineCode("Resistencia")} - " +
                $"{Formatter.InlineCode("Agilidade")} - { Formatter.InlineCode("Vitalidade")}");
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
                        player.Character.Atributos.Forca += quantityResponse.Result;
                        attribute = "força";
                        continueLoop = false;
                        break;

                    case "resistencia":
                        player.Character.Atributos.Resistencia += quantityResponse.Result;
                        attribute = "resistencia";
                        continueLoop = false;
                        break;

                    case "agilidade":
                        player.Character.Atributos.Agilidade += quantityResponse.Result;
                        attribute = "agilidade";
                        continueLoop = false;
                        break;

                    case "vitalidade":
                        player.Character.Atributos.Vitalidade += quantityResponse.Result;
                        attribute = "vitalidade";
                        continueLoop = false;
                        break;
                }
            }

            player.Character.Atributos.PontosLivreAtributo -= quantityResponse.Result;
            database.StopExecutingInteractivity(ctx);
            await database.ReplacePlayerAsync(player);
            await ctx.ResponderAsync($"você atribuiu {Formatter.Bold(quantityResponse.Result.ToString())} em {attribute.Titulo()}");
        }
    }
}
