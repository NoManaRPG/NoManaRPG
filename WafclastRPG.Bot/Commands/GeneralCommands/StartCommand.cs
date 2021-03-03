using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Database;
using WafclastRPG.Bot.Extensions;
using WafclastRPG.Game.Entities;

namespace WafclastRPG.Bot.Commands.GeneralCommands
{
    public class StartCommand : BaseCommandModule
    {
        public BotDatabase banco;

        [Command("comecar")]
        [Aliases("start")]
        [Description("Cria um personagem para o bot poder usar em outros comandos.")]
        [Usage("comecar")]
        public async Task StartCommandAb(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();

            Task<bool> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player != null)
                        return Task.FromResult(false);

                    var newPlayer = new WafclastPlayer(ctx.User.Id);
                    await session.InsertPlayerAsync(newPlayer);
                    return Task.FromResult(true);
                });
            };

            if (await result == true)
                await ctx.ResponderAsync($"personagem criado com sucesso! Obrigado por escolher Wafclast!");
            else
                await ctx.ResponderAsync($"você já tem um personagem criado! Tente usar o comando {Formatter.InlineCode("local")}!");
        }
    }
}
