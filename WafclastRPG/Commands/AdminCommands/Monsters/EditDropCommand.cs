using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.DataBases;
using MongoDB.Bson;
using MongoDB.Driver;
using WafclastRPG.Entities.Monsters;

namespace WafclastRPG.Commands.AdminCommands.Monsters
{
    public class EditDropCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("editdrop")]
        [Description("Permite editar um drop de um monstro.")]
        [Usage("editdrop <posição> <monstro> <item> <chance> <quantidade minima> <quantidade máxima>")]
        [RequireOwner]
        public async Task EditDropCommandAsync(CommandContext ctx, int posicao, string monsterIdString, string itemIdString, double chance, int quantMin, int quantMax)
        {
            await ctx.TriggerTypingAsync();

            Response response;
            using (var session = await database.StartDatabaseSessionAsync())
                response = await session.WithTransactionAsync(async (s, ct) =>
                {
                    if (!ObjectId.TryParse(monsterIdString, out var monsterId))
                        return new Response("o ID do monstro está inválido!");

                    if (!ObjectId.TryParse(itemIdString, out var itemId))
                        return new Response("o ID do item está inválido!");

                    var monster = await database.CollectionMonsters.Find(session.Session, x => x.Id == monsterId).FirstOrDefaultAsync();
                    if (monster == null)
                        return new Response("não encontrei este monstro, você informou o ID correto?");

                    var item = await session.FindItemAsync(itemId);
                    if (item == null)
                        return new Response("não encontrei este item, você informou o ID correto?");

                    var drop = new DropChance
                    {
                        Id = item.Id,
                        Chance = chance / 100,
                        MinQuantity = quantMin,
                        MaxQuantity = quantMax,
                    };

                    monster.DropChances[posicao] = drop;

                    await session.ReplaceAsync(monster);

                    var embed = new DiscordEmbedBuilder();
                    embed.WithTitle($"{monster.Name} editado!");
                    embed.AddField("Item".Titulo(), item.Name, true);
                    embed.AddField("Chance".Titulo(), $"{(drop.Chance * 100):N2}%", true);
                    embed.AddField("Quantia".Titulo(), $"{drop.MinQuantity} ~ {drop.MaxQuantity}", true);
                    embed.WithColor(DiscordColor.Yellow);

                    return new Response(embed);
                });

            if (!string.IsNullOrWhiteSpace(response.Message))
            {
                await ctx.ResponderAsync(response.Message);
                return;
            }

            await ctx.ResponderAsync(response.Embed.Build());
        }
    }
}
