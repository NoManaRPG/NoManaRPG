using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Bot.Entidades;
using WafclastRPG.Game;
using WafclastRPG.Game.Entidades;

namespace WafclastRPG.Bot.Extensoes
{
    public static class BancoExtension
    {
        public static async Task<Tuple<bool, Sessao>> ExisteJogadorAsync(this Banco banco, CommandContext ctx, bool esperar = false)
        {
            var balde = banco.GetBalde(ctx.User.Id);
            if (esperar)
                await balde.WaitAsync();

            var jogador = await banco.GetJogadorAsync(ctx.User.Id);
            if (jogador == null)
            {
                if (esperar)
                    balde.Release();
                await ctx.RespondAsync($"Bem-vindo! {ctx.User.Mention} antes de começar, crie um personagem digitando {Formatter.InlineCode("!criar-personagem")}.");
                return new Tuple<bool, Sessao>(false, null);
            }

            var sessao = new Sessao(jogador, balde, banco);
            return new Tuple<bool, Sessao>(true, sessao);
        }

        public static Task<WafclastJogador> GetJogadorAsync(this Banco banco, CommandContext ctx)
            => banco.GetJogadorAsync(ctx.User.Id);

        public static Task<WafclastJogador> GetJogadorAsync(this Banco banco, DiscordUser user)
            => banco.GetJogadorAsync(user.Id);
    }
}
