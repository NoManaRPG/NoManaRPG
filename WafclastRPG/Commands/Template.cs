using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands
{
    public class Template : BaseCommandModule
    {
        public Database banco;

        [Command("")]
        [Description("Permite ")]
        [Usage("")]
        public async Task TemplateAsync(CommandContext ctx)
        {
            var timer = new Stopwatch();
            timer.Start();

            await ctx.TriggerTypingAsync();

            Task<Response> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return Task.FromResult(new Response() { IsPlayerFound = false });


                    return Task.FromResult(new Response());
                });
            var _response = await result;

            if (_response.IsPlayerFound == false)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            timer.Stop();
        }

        private class Response
        {
            public bool IsPlayerFound = true;
        }
    }
}
