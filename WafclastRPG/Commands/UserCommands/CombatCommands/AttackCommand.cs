using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using WafclastRPG.Attributes;
using WafclastRPG.DataBases;
using WafclastRPG.Extensions;

namespace WafclastRPG.Commands.UserCommands.CombatCommands
{
    public class AttackCommand : BaseCommandModule
    {
        public DataBase database;

        [Command("atacar")]
        [Aliases("at")]
        [Description("Permite executar um ataque físico em um monstro.")]
        [Usage("atacar")]
        public async Task AttackCommandAsync(CommandContext ctx)
        {
            database.Users.TryGetValue(ctx.User.Id, out var player);

            player.Character.ReceiveDamage(20);

            await ctx.ResponderAsync($"{player.Character.ShowLife()}");
        }
    }
}
