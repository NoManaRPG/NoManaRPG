using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
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
        public async Task StartCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var rm = await banco.CollectionMaps.Find(x => x.Id == ctx.Channel.Id).FirstOrDefaultAsync();
            if (rm == null || rm.Tipo != WafclastMapaType.Cidade)
            {
                await ctx.ResponderAsync("você precisa criar um personagem na cidade!");
                return;
            }

            Task<bool> result;
            using (var session = await this.banco.StartDatabaseSessionAsync())
            {
                result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var player = await session.FindPlayerAsync(ctx.User);
                    if (player != null)
                        return Task.FromResult(false);

                    var newPlayer = new WafclastPlayer(ctx.User.Id);
                    newPlayer.Character.LocalId = rm.Id;
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
