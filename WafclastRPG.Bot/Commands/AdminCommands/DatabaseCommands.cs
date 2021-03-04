using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Bot.Database;
using WafclastRPG.Bot.Extensions;

namespace WafclastRPG.Bot.Commands.AdminCommands
{
    public class DatabaseCommands : BaseCommandModule
    {
        public BotDatabase banco;

        [Command("deletar-user")]
        [Description("Apaga o usuario")]
        [RequireOwner]
        public async Task DeletarUser(CommandContext ctx, DiscordUser user)
        {
            await ctx.TriggerTypingAsync();
            var result = await banco.CollectionJogadores.DeleteOneAsync(x => x.Id == user.Id);
            if (result.DeletedCount >= 1)
                await ctx.ResponderAsync("usuario deletado!");
            else
                await ctx.ResponderAsync("não foi possível deletar!");
        }
    }
}
