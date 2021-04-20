using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;

namespace WafclastRPG.Commands.AdminCommands
{
    class AdminCommand : BaseCommandModule
    {
        [Command("admin")]
        [Description("Exibe todos os comandos que o bot reconhece de administrador.")]
        [Usage("admin")]
        public async Task CommandsAsync(CommandContext ctx)
        {
            var str = new StringBuilder();
            str.AppendLine("[Admin]");
            str.Append("deletarU, ");
            str.Append("monstroEC, ");
            str.Append("monstroEDROP, ");
            str.Append("monstroV, ");
            str.Append("andarV, ");
            str.Append("itemEC, ");
            str.Append("itensV, ");

            await ctx.RespondAsync(Formatter.BlockCode(str.ToString(), "css"));
        }
    }
}
