using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Entities;
using WafclastRPG.Entities.Monsters;
using WafclastRPG.Enums;
using WafclastRPG.Extensions;
using WafclastRPG.Properties;

namespace WafclastRPG.Commands.PlayerCommands
{
    public class LootCommand : BaseCommandModule
    {
        public DataBase banco;

        [Command("saquear")]
        [Aliases("loot")]
        [Description("Permite saquear corpos de monstros mortos.")]
        [Usage("saquear [ ID ]")]
        [Example("saquear 1", "Faz você procurar por itens no corpo do monstro de ID 1.")]
        public async Task LootCommandAsync(CommandContext ctx, int monsterId = 1)
        {
            var timer = new Stopwatch();
            timer.Start();

            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await banco.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                 {
                     //Find player
                     var player = await session.FindAsync(ctx.User);
                     //Set language
                     Thread.CurrentThread.CurrentUICulture = new CultureInfo(player.Language);
                     //await ctx.ResponderAsync("asd");
                     if (player.Character == null)
                         return new Response(Messages.NaoEscreveuComecar);
                     return new Response(Messages.NaoEscreveuComecar);
                 });

            //foreach (var item in target.Drops)
            //{
            //    if (random.Chance(item.Chance))
            //    {
            //        var quant = random.Sortear(item.MinQuantity, item.MaxQuantity);
            //        var _item = await banco.FindItemByObjectIdAsync(item.ItemId, ctx.Guild.Id);
            //        await player.ItemAdd(_item, quant);
            //        str.AppendLine($"+{_item.Name}*x*{quant}");
            //    }
            //}
        }
    }
}