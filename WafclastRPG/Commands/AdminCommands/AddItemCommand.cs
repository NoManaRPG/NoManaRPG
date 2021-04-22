using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.DataBases;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.AdminCommands
{
    public class AddItemCommand : BaseCommandModule
    {
        public DataBase database;
        public TimeSpan timeoutoverride = TimeSpan.FromMinutes(2);

        [Command("additem")]
        [Description("Permite adicionar itens para um jogador.")]
        [Usage("additem <@jogador> <quantidade> <item>")]
        [RequireOwner]
        public async Task CreateFabricationCommandAsync(CommandContext ctx, DiscordUser user, ulong quantidade, [RemainingText] string itemName)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    var item = await session.FindItemAsync(itemName, ctx.Client.CurrentUser);
                    if (item == null)
                        return new Response("item não encontrado!");

                    var player = await session.FindPlayerAsync(user);
                    if (player == null)
                        return new Response(Messages.JogadorAlvoNaoCriouPersonagem);

                    await player.AddItemAsync(item, quantidade);

                    return new Response($"adicionado {quantidade} x {item.Name} para {user.Mention}.");
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
                await ctx.ResponderAsync(response.Message);

            if (response.Embed != null)
                await ctx.ResponderAsync(response.Embed?.Build());
        }
    }
}
