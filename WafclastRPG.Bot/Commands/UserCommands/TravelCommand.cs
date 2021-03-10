using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Bot.Atributos;
using WafclastRPG.Bot.Database;
using WafclastRPG.Bot.Extensions;

namespace WafclastRPG.Bot.Commands.UserCommands
{
    public class TravelCommand : BaseCommandModule
    {
        public BotDatabase banco;

        [Command("viajar")]
        [Description("Permite viajar para outra região.")]
        [Usage("viajar")]
        public async Task TravelCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var map = await banco.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
            if (map == null)
            {
                await ctx.ResponderAsync("você não pode viajar até aqui!");
                return;
            }


            Task<Response> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player == null)
                        return Task.FromResult(new Response());

                    if (player.Character.LocalId == ctx.Channel.Id)
                        return Task.FromResult(new Response() { IsPlayerFound = true, IsSamePlace = true });


                    player.Character.LocalId = ctx.Channel.Id;
                    player.Character.ServerId = ctx.Guild.Id;
                    await player.SaveAsync();

                    return Task.FromResult(new Response() { IsPlayerFound = true, IsSamePlace = false });
                });
            };
            var _response = await result;

            if (_response.IsPlayerFound == false)
            {
                await ctx.ResponderAsync($"você precisa usar o comando {Formatter.InlineCode("comecar")} antes!");
                return;
            }

            if (_response.IsSamePlace)
            {
                await ctx.ResponderAsync("não tem como viajar para o mesmo lugar!");
                return;
            }

            await ctx.ResponderAsync($"você acaba de viajar para {Formatter.Bold(ctx.Channel.Name)}!");
        }

        public class Response
        {
            public bool IsPlayerFound = false;
            public bool IsSamePlace = false;
        }
    }
}
