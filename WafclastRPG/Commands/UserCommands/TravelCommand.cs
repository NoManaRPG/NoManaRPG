using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.Entities;
using WafclastRPG.Enums;

namespace WafclastRPG.Commands.UserCommands
{
    public class TravelCommand : BaseCommandModule
    {
        public Database banco;

        [Command("viajar")]
        [Aliases("v", "travel")]
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
                        return Task.FromResult(new Response() { IsPlayerFound = false });

                    if (player.Character.Localization.ChannelId == ctx.Channel.Id)
                        return Task.FromResult(new Response() { IsSamePlace = true });

                    if (player.Character.Karma < 0)
                        if (map.Tipo == MapType.Cidade)
                            return Task.FromResult(new Response() { IsKarmaNegative = true });

                    player.Character.Localization = new WafclastLocalization(ctx.Channel.Id, ctx.Guild.Id);
                    await player.SaveAsync();

                    return Task.FromResult(new Response());
                });
            };
            var _response = await result;

            if (_response.IsPlayerFound == false)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            if (_response.IsSamePlace)
            {
                await ctx.ResponderAsync("não tem como viajar para o mesmo lugar! Você precisa ir em outro canal de texto.");
                return;
            }

            if (_response.IsKarmaNegative)
            {
                await ctx.ResponderAsync("seu Karma está negativo, os guardas não deixarão você entrar na cidade!");
                return;
            }

            await ctx.ResponderAsync($"você acaba de chegar em {Formatter.Bold(ctx.Channel.Name)}!");
        }

        public class Response
        {
            public bool IsPlayerFound = true;
            public bool IsSamePlace = false;
            public bool IsKarmaNegative = false;
        }
    }
}
