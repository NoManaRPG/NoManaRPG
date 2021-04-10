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
                     var player = await session.FindAsync(ctx.User);
                     Thread.CurrentThread.CurrentUICulture = new CultureInfo(player.Language);

                     if (player.Character == null)
                         return new Response(Messages.NaoEscreveuComecar);

                     if (player.Character.Localization != ctx.Channel)
                         return new Response(Messages.ComandoEmLocalizacaoDiferente);

                     var map = await session.FindAsync(ctx.Channel);
                     if (map == null)
                         return new Response(Messages.CanalNaoEsMapa);

                     if (map.Tipo == MapType.Cidade)
                         return new Response("como os guardas não deixam entrar monstros na cidade, não tem nada para saquear!");

                     var monster = await session.FindAsync(new WafclastMonsterBase(ctx.Channel.Id, monsterId));
                     if (monster == null)
                         return new Response("não existe nenhum monstro com este ID!");

                     if (monster.Life.CurrentValue > 0)
                         return new Response($"o monstro ainda está vivo!");

                     if (monster.ItsPillaged)
                         return new Response("o monstro já foi saqueado por outra pessoa!");

                     var random = new Random();
                     var str = new StringBuilder();
                     foreach (var drop in monster.ChanceDrops)
                     {
                         if (random.Chance(drop.Chance))
                         {
                             var quantity = random.Sortear(drop.MinQuantity, drop.MaxQuantity);
                             var item = await banco.FindAsync(drop.Id);
                             await session.InsertAsync(item, quantity, player);
                             str.AppendLine($"+{quantity} {item.Name.Titulo()}");
                         }
                     }
                     monster.Restart();
                     await session.ReplaceAsync(monster);
                     await session.ReplaceAsync(player);

                     var embed = new DiscordEmbedBuilder();
                     if (str.Length > 0)
                         embed.WithDescription(str.ToString());
                     else
                         embed.WithDescription("Você procurou mas não encontrou nada valioso!");
                     return new Response(embed);
                 });

            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                await ctx.ResponderAsync(response.Message);
                return;
            }

            timer.Stop();
            response.Embed.WithFooter($"Tempo de resposta: {timer.Elapsed.Seconds}.{timer.ElapsedMilliseconds + ctx.Client.Ping}s.");
            await ctx.RespondAsync(ctx.User.Mention, response.Embed.Build());
        }
    }
}