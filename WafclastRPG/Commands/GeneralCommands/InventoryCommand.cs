using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
using WafclastRPG.DataBases;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.Entities;
using WafclastRPG.Enums;
using DSharpPlus.Entities;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class InventoryCommand : BaseCommandModule
    {
        public Database banco;

        [Command("inventario")]
        [Description("Permite ver os seus itens.")]
        [Usage("inventario")]
        [Aliases("inv", "inventory")]
        public async Task InventoryCommandAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var player = await banco.FindPlayerAsync(ctx.User);
            if (player == null)
            {
                await ctx.ResponderAsync(Strings.NovoJogador);
                return;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithAuthor($"{ctx.User.Username} [Nv.{player.Character.Level}]", iconUrl: ctx.User.AvatarUrl);
            embed.WithDescription($"{Emojis.Coins} {player.Character.Coins.ToString()}");
            embed.AddField("Itens".Titulo(), "Vazio..");

            await ctx.ResponderAsync(embed.Build());
        }
    }
}
