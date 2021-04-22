using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.Extensions;
using WafclastRPG.DataBases;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;

namespace WafclastRPG.Commands.AdminCommands.Monsters
{
    public class SeeMonsterCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("vermonstro")]
        [Description("Exibe as informações de um monstro.")]
        [Usage("vermonstro <monstro>")]
        [RequireOwner]
        public async Task SeeMonsterCommandAsync(CommandContext ctx, string monsterIdString)
        {
            await ctx.TriggerTypingAsync();

            if (!ObjectId.TryParse(monsterIdString, out var monsterId))
            {
                await ctx.ResponderAsync("o ID do monstro está inválido!");
                return;
            }

            var monster = await database.CollectionMonsters.Find(x => x.Id == monsterId).FirstOrDefaultAsync();
            if (monster == null)
            {
                await ctx.ResponderAsync("não encontrei este monstro, você informou o ID correto?");
                return;
            }

            var embed = new DiscordEmbedBuilder();
            embed.WithTitle(monster.Name);
            embed.WithDescription($"ID `{monster.Id}`" +
                $"\nAndar {monster.FloorLevel}");
            embed.AddField("Vida", monster.Life.MaxValue.ToString("N2"), true);
            embed.AddField("Armadura", monster.Armour.MaxValue.ToString("N2"), true);
            embed.AddField("Dano físico", monster.PhysicalDamage.MaxValue.ToString("N2"), true);
            embed.AddField("Evasão", monster.Evasion.MaxValue.ToString("N2"), true);
            embed.AddField("Precisão", monster.Accuracy.MaxValue.ToString("N2"), true);

            var str = new StringBuilder();
            int index = 0;
            foreach (var item in monster.DropChances)
            {
                var drop = await database.CollectionItems.Find(x => x.Id == item.Id).FirstOrDefaultAsync();
                if (item.MinQuantity == item.MaxQuantity)
                    str.AppendLine($"[{index}] - {item.Chance * 100}% {item.MaxQuantity} x {drop.Name} `{drop.Id}`");
                else
                    str.AppendLine($"[{index}] - {item.Chance * 100}% de {item.MinQuantity} ~ {item.MaxQuantity} x {drop.Name} `{drop.Id}`");
                index++;
            }

            embed.AddField("Drops", str.ToString());
            await ctx.ResponderAsync(embed.Build());
        }
    }
}
