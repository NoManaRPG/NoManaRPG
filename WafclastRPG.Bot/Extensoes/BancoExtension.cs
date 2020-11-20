using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Bot.Entidades;

namespace WafclastRPG.Bot.Extensoes
{
    public static class BancoExtension
    {
        public static Task<BotJogador> GetJogadorAsync(this Banco banco, CommandContext ctx)
        => banco.GetJogadorAsync(ctx.User.Id, ctx.User);

        public static Task<BotJogador> GetJogadorAsync(this Banco banco, DiscordUser user)
            => banco.GetJogadorAsync(user.Id, user);
    }
}
