using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WafclastRPG.Extensions;
using WafclastRPG;
using WafclastRPG.Entities;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace WafclastRPG.Commands.AdminCommands
{
    public class TestCommands : BaseCommandModule
    {
        public Database banco;

        [Command("testar")]
        [RequireOwner]
        public async Task CommmandEnumTest(CommandContext ctx)
        {
            WafclastLocalization g = new WafclastLocalization();
            WafclastLocalization h = new WafclastLocalization(1, 2);

            if (g.Equals( h))
            {
                await ctx.ResponderAsync("asd");
            }
        }

        public enum enumtest
        {
            [Description("Valor 1")]
            Valor1,
            [Description("Valor com Valor 1")]
            ValorComValor1
        }

        [Command("editar")]
        [RequireOwner]
        public async Task editar(CommandContext ctx, ulong id)
        {
            Task<bool> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var user = await session.FindPlayerAsync(id);
                    if (user.Character.Level >= 4)
                        return Task.FromResult(false);
                    user.Character.Level = 10;
                    await user.SaveAsync();
                    return Task.FromResult(true);
                });
            }

            if (await result == true)
                await ctx.ResponderAsync("alterado");
            else
                await ctx.ResponderAsync("não alterado");
        }

        [Command("zerar")]
        [RequireOwner]
        public async Task asd(CommandContext ctx)
        {
            Task<bool> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var user = await session.FindPlayerAsync(ctx.User);
                    user.Character.Level = 0;
                    await user.SaveAsync();
                    return Task.FromResult(true);
                });
            };

            if (await result == true)
                await ctx.ResponderAsync("zerado");
        }
    }
}
