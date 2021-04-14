using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Text;
using System.Threading.Tasks;
using WafclastRPG.Attributes;

namespace WafclastRPG.Commands.GeneralCommands
{
    public class CommandsCommand : BaseCommandModule
    {
        [Command("comandos")]
        [Aliases("commands")]
        [Description("Exibe todos os comandos que o bot reconhece.")]
        [Usage("comandos")]
        public async Task CommandsAsync(CommandContext ctx)
        {
            var str = new StringBuilder();
            str.AppendLine("[Admin]");
            str.AppendLine("[Geral]");
            str.Append("comandos, ");
            str.Append("ajuda, ");
            str.Append("info, ");
            str.AppendLine();
            str.AppendLine("[Habilidades]");
            str.Append("minerar, ");
            str.AppendLine();
            str.AppendLine("[Usuário]");
            str.Append("explorar, ");
            str.Append("atacar, ");
            str.Append("status, ");
            str.Append("inventario, ");

            await ctx.RespondAsync(Formatter.BlockCode(str.ToString(), "css"));
        }
    }
}
