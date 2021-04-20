using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.UserCommands
{
    public class ReminderCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("despertador")]
        [Description("Permite ativar o despertador pra alguns comandos.")]
        public async Task UseCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return new Response(Messages.NaoEscreveuComecar);

                    if (player.Reminder == false)
                        player.Reminder = true;
                    else
                        player.Reminder = false;

                    await player.SaveAsync();

                    return new Response("o despertador está agora: " + (player.Reminder? "Ativado" : "Desativado"));
                });

            await ctx.ResponderAsync(response.Message);
        }
    }
}
