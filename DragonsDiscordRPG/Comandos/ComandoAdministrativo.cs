using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Comandos
{
    public class ComandoAdministrativo : BaseCommandModule
    {
        

        [Command("purge")]
        [RequireUserPermissions(Permissions.Administrator)]
        public async Task PurgeAsync(CommandContext ctx, int quantidade)
            => await ctx.Channel.DeleteMessagesAsync(await ctx.Channel.GetMessagesAsync(quantidade + 1));
    }
}
