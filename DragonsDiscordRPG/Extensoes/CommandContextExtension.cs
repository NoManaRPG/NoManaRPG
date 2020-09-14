using DragonsDiscordRPG.Entidades;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;

namespace DragonsDiscordRPG.Extensoes
{
    public static class CommandContextExtension
    {
        public static async Task<bool> JogadorNaoExisteAsync(this CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            RPJogador jogador = await ModuloBanco.GetJogadorAsync(ctx);
            if (jogador == null)
            {
                await ctx.RespondAsync($"{ctx.User.Mention}, você precisa criar um personagem com o comando `criar-personagem`!");
                return true;
            }
            return false;
        }

        public static async Task ExecutarAjudaAsync(this CommandContext ctx)
        {
            var cmd = ctx.CommandsNext.FindCommand($"ajuda {ctx.Command.Name}", out var args);
            var cfx = ctx.CommandsNext.CreateContext(ctx.Message, "!", cmd, args);
            await ctx.CommandsNext.ExecuteCommandAsync(cfx);
        }
    }
}
