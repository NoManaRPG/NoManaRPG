using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.Enums;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class TravelCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("viajar")]
        [Aliases("v", "travel")]
        [Description("Permite viajar para outra região.")]
        [Usage("viajar")]
        public async Task TravelCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            var map = await banco.FindAsync(ctx.Channel);
            if (map == null)
            {
                await ctx.ResponderAsync("você só pode viajar entre mapas, e aqui não é um.");
                return;
            }

            Response response;
            using (var session = await this.banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindAsync(ctx.User);
                    if (player.Character == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    if (player.Character.Localization.ChannelId == ctx.Channel.Id)
                        return new Response("não tem como viajar para o mesmo lugar! Você precisa ir em outro canal de texto.");

                    if (player.Character.Karma < 0 && map.Tipo == MapType.Cidade)
                        return new Response("seu Karma está negativo, os guardas não deixarão você entrar na cidade!");

                    if (player.Character.Localization.ServerId != ctx.Guild.Id)
                        return new Response($"o seu personagem é de outro servidor! Use o comando {Formatter.InlineCode("comecar")} para criar um novo personagem!");

                    player.Character.Localization.ChannelId = ctx.Channel.Id;
                    await session.ReplaceAsync(player);

                    return new Response($"você acaba de chegar em {Formatter.Bold(ctx.Channel.Name)}!");
                });
            await ctx.ResponderAsync(response.Message);
        }
    }
}
