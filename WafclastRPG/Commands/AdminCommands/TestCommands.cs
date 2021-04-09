using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Extensions;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
using WafclastRPG.DataBases;
using System.Text;
using WafclastRPG.Properties;
using System.Threading;
using System.Resources;
using System.Globalization;
using WafclastRPG.Entities;

namespace WafclastRPG.Commands.AdminCommands
{
    public class TestCommands : BaseCommandModule
    {
        public DataBase banco;

        [Command("lang")]
        public async Task CommmandEnumTest(CommandContext ctx)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            await ctx.ResponderAsync(Messages.NaoEscreveuComecar);
        }

        [Command("emojis")]
        [RequireOwner]
        public async Task EmojisAsync(CommandContext ctx)
        {
            var emojis = await ctx.Guild.GetEmojisAsync();
            var str = new StringBuilder();
            foreach (var emoji in emojis)
            {
                //<a:NAME:ID>
                str.AppendLine($"{emoji} : {Formatter.InlineCode($"<a:{emoji.Name}:{emoji.Id}")}");
            }

            await ctx.RespondAsync(str.ToString());
        }

        [Command("teste")]
        [RequireOwner]
        public async Task TesteAsync(CommandContext ctx)
        {
            var PhysicalDamage = new WafclastStatePoints(8);
            PhysicalDamage.MultValue = 20 * 0.2M;
            PhysicalDamage.MultValue += 100;
        }
    }
}
